using AutoMapper;

using Services.Communication.Http.Broker.Department.Storage.Models;

using Infrastructure.Caching.Redis;
using Infrastructure.Communication.Http.Wrapper;
using Infrastructure.Communication.Http.Wrapper.Disposing;
using Infrastructure.Localization.Providers;
using Infrastructure.Transaction.Recovery;
using Infrastructure.Transaction.UnitOfWork.EntityFramework;

using Microsoft.EntityFrameworkCore;

using Services.Api.Business.Departments.Storage.Configuration.Persistence;
using Services.Api.Business.Departments.Storage.Entities.EntityFramework;
using Services.Api.Business.Departments.Storage.Repositories.EntityFramework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Storage.Services
{
    /// <summary>
    /// Stok işlemleri iş mantığı sınıfı
    /// </summary>
    public class StockService : BaseService, IRollbackableAsync, IAsyncDisposable, IDisposableInjectionsAsync
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// İçerisinde çalışılan servisin adı
        /// </summary>
        public override string ServiceName => "Services.Api.Business.Departments.Storage.Services.StockService";

        /// <summary>
        /// Önbelleğe alınan stokların önbellekteki adı
        /// </summary>
        private const string CACHED_STOCKS_KEY = "Services.Api.Business.Departments.Storage.Stocks";

        /// <summary>
        /// Servisin ait olduğu api servisinin adı
        /// </summary>
        public override string ApiServiceName => "Services.Api.Business.Departments.Storage";

        /// <summary>
        /// Rediste tutulan önbellek yönetimini sağlayan sınıf
        /// </summary>
        private readonly RedisCacheDataProvider _redisCacheDataProvider;

        /// <summary>
        /// Mapping işlemleri için mapper nesnesi
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// İşlem tablosu için repository sınıfı
        /// </summary>
        private readonly TransactionRepository _transactionRepository;

        /// <summary>
        /// İşlem öğesi tablosu için repository sınıfı
        /// </summary>
        private readonly TransactionItemRepository _transactionItemRepository;

        /// <summary>
        /// Veritabanı iş birimi nesnesi
        /// </summary>
        private readonly IUnitOfWork<StorageContext> _unitOfWork;

        /// <summary>
        /// Müşteriler repository sınıfı
        /// </summary>
        private readonly StockRepository _stockRepository;

        /// <summary>
        /// Dil çeviri sağlayıcısı sınıf
        /// </summary>
        private readonly TranslationProvider _translationProvider;

        /// <summary>
        /// Stok işlemleri iş mantığı sınıfı
        /// </summary>
        /// <param name="mapper">Mapping işlemleri için mapper nesnesi</param>
        /// <param name="unitOfWork">Veritabanı iş birimi nesnesi</param>
        /// <param name="translationProvider">Dil çeviri sağlayıcısı sınıf nesnesi</param>
        /// <param name="redisCacheDataProvider">Rediste tutulan önbellek yönetimini sağlayan sınıf nesnesi</param>
        /// <param name="transactionRepository">İşlem tablosu için repository sınıfı nesnesi</param>
        /// <param name="transactionItemRepository">İşlem öğesi tablosu için repository sınıfı nesnesi</param>
        /// <param name="stockRepository">Stoklar repository sınıfı nesnesi</param>
        public StockService(
            IMapper mapper,
            IUnitOfWork<StorageContext> unitOfWork,
            TranslationProvider translationProvider,
            RedisCacheDataProvider redisCacheDataProvider,
            TransactionRepository transactionRepository,
            TransactionItemRepository transactionItemRepository,
            StockRepository stockRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _translationProvider = translationProvider;
            _redisCacheDataProvider = redisCacheDataProvider;

            _transactionRepository = transactionRepository;
            _transactionItemRepository = transactionItemRepository;
            _stockRepository = stockRepository;
        }

        /// <summary>
        /// Bir işlemi geri almak için yedekleme noktası oluşturur
        /// </summary>
        /// <param name="rollback">İşlemin yedekleme noktası nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns>TIdentity işlemin geri dönüş tipidir</returns>
        public async Task CreateCheckpointAsync(RollbackModel rollback, CancellationTokenSource cancellationTokenSource)
        {
            RollbackEntity rollbackEntity = _mapper.Map<RollbackModel, RollbackEntity>(rollback);

            List<RollbackItemEntity> rollbackItemEntities = _mapper.Map<List<RollbackItemModel>, List<RollbackItemEntity>>(rollback.RollbackItems);

            foreach (var rollbackItemEntity in rollbackItemEntities)
            {
                rollbackItemEntity.TransactionIdentity = rollbackEntity.TransactionIdentity;

                await _transactionItemRepository.CreateAsync(rollbackItemEntity, cancellationTokenSource);
            }

            await _transactionRepository.CreateAsync(rollbackEntity, cancellationTokenSource);
        }

        /// <summary>
        /// Bir işlemi geri alır
        /// </summary>
        /// <param name="rollback">Geri alınacak işlemin yedekleme noktası nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns>TIdentity işlemin geri dönüş tipidir</returns>
        public async Task RollbackTransactionAsync(RollbackModel rollback, CancellationTokenSource cancellationTokenSource)
        {
            foreach (var rollbackItem in rollback.RollbackItems)
            {
                switch (rollbackItem.DataSet?.ToString())
                {
                    case StockRepository.TABLE_NAME:
                        if (rollbackItem.RollbackType == RollbackType.Delete)
                        {
                            await _stockRepository.DeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Insert)
                        {
                            await _stockRepository.UnDeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Update)
                        {
                            await _stockRepository.SetAsync((int)rollbackItem.Identity, rollbackItem.Name, rollbackItem.OldValue, cancellationTokenSource);
                        }
                        else
                            throw new Exception(
                                await _translationProvider.TranslateAsync("Tanimsiz.Geri.Alma", Region, cancellationToken: cancellationTokenSource.Token));
                        break;
                    default:
                        break;
                }
            }

            await _transactionRepository.SetRolledbackAsync(rollback.TransactionIdentity, cancellationTokenSource);

            await _unitOfWork.SaveAsync(cancellationTokenSource);
        }

        public async Task DisposeInjectionsAsync()
        {
            _redisCacheDataProvider.Dispose();
            await _stockRepository.DisposeAsync();
            await _transactionItemRepository.DisposeAsync();
            await _transactionRepository.DisposeAsync();
            await _unitOfWork.DisposeAsync();
            _translationProvider.Dispose();
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <returns></returns>
        public ValueTask DisposeAsync()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

            return ValueTask.CompletedTask;
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    disposed = true;
                }
            }
        }

        public async Task<StockModel> GetStockAsync(int productId, CancellationTokenSource cancellationTokenSource)
        {
            if (_redisCacheDataProvider.TryGetValue(CACHED_STOCKS_KEY, out List<StockModel> cachedStocks)
                 &&
                 cachedStocks != null && cachedStocks.Any())
            {
                return cachedStocks.FirstOrDefault(x => x.ProductId == productId);
            }

            List<StockEntity> stocks = await _stockRepository.GetListAsync(cancellationTokenSource);

            List<StockModel> mappedStocks = _mapper.Map<List<StockEntity>, List<StockModel>>(stocks);

            _redisCacheDataProvider.Set(CACHED_STOCKS_KEY, mappedStocks);

            return mappedStocks.FirstOrDefault(x => x.ProductId == productId);
        }

        public async Task<int> CreateStockAsync(StockModel stockModel, CancellationTokenSource cancellationTokenSource)
        {
            StockEntity existingStock = await _stockRepository.GetAsQueryable().FirstOrDefaultAsync(x => x.ProductId == stockModel.ProductId);

            if (existingStock != null)
            {
                await CreateCheckpointAsync(
                    rollback: new RollbackModel()
                    {
                        TransactionType = TransactionType.Update,
                        TransactionDate = DateTime.Now,
                        TransactionIdentity = TransactionIdentity,
                        RollbackItems = new List<RollbackItemModel>
                        {
                            new RollbackItemModel()
                            {
                                Identity = existingStock.Id,
                                DataSet = StockRepository.TABLE_NAME,
                                RollbackType = RollbackType.DecreaseValue,
                                Difference = stockModel.Amount,
                                Name = nameof(existingStock.Amount)
                            }
                        }
                    },
                    cancellationTokenSource: cancellationTokenSource);

                existingStock.Amount += stockModel.Amount;

                await _unitOfWork.SaveAsync(cancellationTokenSource);

                if (_redisCacheDataProvider.TryGetValue(CACHED_STOCKS_KEY, out List<StockModel> cachedStocks) && cachedStocks != null)
                {
                    cachedStocks.FirstOrDefault(x => x.ProductId == stockModel.ProductId).Amount += stockModel.Amount;

                    _redisCacheDataProvider.Set(CACHED_STOCKS_KEY, cachedStocks);
                }

                return existingStock.Id;
            }
            else
            {
                StockEntity mappedStockEntity = _mapper.Map<StockModel, StockEntity>(stockModel);

                await _stockRepository.CreateAsync(mappedStockEntity, cancellationTokenSource);

                await CreateCheckpointAsync(
                    rollback: new RollbackModel()
                    {
                        TransactionType = TransactionType.Insert,
                        TransactionDate = DateTime.Now,
                        TransactionIdentity = TransactionIdentity,
                        RollbackItems = new List<RollbackItemModel>
                        {
                            new RollbackItemModel()
                            {
                                Identity = mappedStockEntity.Id,
                                DataSet = StockRepository.TABLE_NAME,
                                RollbackType = RollbackType.Delete
                            }
                        }
                    },
                    cancellationTokenSource: cancellationTokenSource);

                await _unitOfWork.SaveAsync(cancellationTokenSource);

                stockModel.ProductId = mappedStockEntity.Id;

                if (_redisCacheDataProvider.TryGetValue(CACHED_STOCKS_KEY, out List<StockModel> cachedStocks) && cachedStocks != null)
                {
                    cachedStocks.Add(stockModel);

                    _redisCacheDataProvider.Set(CACHED_STOCKS_KEY, cachedStocks);
                }

                return mappedStockEntity.Id;
            }
        }

        public async Task<int> DescendProductStockAsync(StockModel stockModel, CancellationTokenSource cancellationTokenSource)
        {
            StockEntity mappedStockEntity = _mapper.Map<StockModel, StockEntity>(stockModel);

            StockEntity stockEntity = await _stockRepository.GetAsQueryable().FirstOrDefaultAsync(x => x.ProductId == mappedStockEntity.ProductId);

            if (stockEntity != null)
            {
                await CreateCheckpointAsync(
                    rollback: new RollbackModel()
                    {
                        TransactionType = TransactionType.Update,
                        TransactionDate = DateTime.Now,
                        TransactionIdentity = TransactionIdentity,
                        RollbackItems = new List<RollbackItemModel>
                        {
                            new RollbackItemModel()
                            {
                                Identity = stockEntity.Id,
                                DataSet = StockRepository.TABLE_NAME,
                                RollbackType = RollbackType.IncreaseValue,
                                Difference = stockModel.Amount,
                                Name = nameof(stockEntity.Amount)
                            }
                        }
                    },
                    cancellationTokenSource: cancellationTokenSource);

                stockEntity.Amount -= mappedStockEntity.Amount;

                if (stockEntity.Amount < 0)
                {
                    throw new Exception("Stok miktarı negatife düşüyor");
                }

                await _unitOfWork.SaveAsync(cancellationTokenSource);

                if (_redisCacheDataProvider.TryGetValue(CACHED_STOCKS_KEY, out List<StockModel> cachedStocks) && cachedStocks != null)
                {
                    cachedStocks.FirstOrDefault(x => x.ProductId == stockModel.ProductId).Amount = stockEntity.Amount;

                    _redisCacheDataProvider.Set(CACHED_STOCKS_KEY, cachedStocks);
                }

                return stockEntity.Id;
            }
            else
                throw new Exception("Ürün kaydı bulunamadı");
        }
    }
}
