using AutoMapper;

using Communication.Http.Department.Storage;
using Communication.Http.Department.Storage.Models;
using Communication.Mq.Rabbit.Publisher.Department.Buying;
using Communication.Mq.Rabbit.Publisher.Department.Buying.Models;
using Communication.Mq.Rabbit.Publisher.Department.Storage;
using Communication.Mq.Rabbit.Publisher.Department.Storage.Models;

using Infrastructure.Caching.Redis;
using Infrastructure.Communication.Http.Broker.Exceptions;
using Infrastructure.Communication.Http.Broker.Models;
using Infrastructure.Communication.Http.Wrapper;
using Infrastructure.Communication.Http.Wrapper.Disposing;
using Infrastructure.Localization.Providers;
using Infrastructure.Transaction.Recovery;
using Infrastructure.Transaction.UnitOfWork.EntityFramework;

using Microsoft.EntityFrameworkCore;

using Services.Business.Departments.Production.Configuration.Persistence;
using Services.Business.Departments.Production.Constants;
using Services.Business.Departments.Production.Entities.EntityFramework;
using Services.Business.Departments.Production.Models;
using Services.Business.Departments.Production.Repositories.EntityFramework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.Production.Services
{
    /// <summary>
    /// Üretim işlemleri iş mantığı sınıfı
    /// </summary>
    public class ProductionService : BaseService, IRollbackableAsync, IAsyncDisposable, IDisposableInjectionsAsync
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// İçerisinde çalışılan servisin adı
        /// </summary>
        public override string ServiceName => "Services.Business.Departments.Production.Services.ProductionService";

        /// <summary>
        /// Servisin ait olduğu api servisinin adı
        /// </summary>
        public override string ApiServiceName => "Services.Business.Departments.Production";

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
        private readonly IUnitOfWork<ProductionContext> _unitOfWork;

        /// <summary>
        /// Ürünler repository sınıfı
        /// </summary>
        private readonly ProductRepository _productRepository;

        /// <summary>
        /// Ürün bağımlılıkları repository sınıfı
        /// </summary>
        private readonly ProductDependencyRepository _productDependencyRepository;

        /// <summary>
        /// Dil çeviri sağlayıcısı sınıf
        /// </summary>
        private readonly TranslationProvider _translationProvider;

        /// <summary>
        /// Stok servisi iletişim sağlayıcı sınıf
        /// </summary>
        private readonly StorageCommunicator _storageCommunicator;

        /// <summary>
        /// Satınalma departmanına alınması istenilen ürün talepleri için kayıt açan sınıf
        /// </summary>
        private readonly CreateProductRequestPublisher _createProductRequestPublisher;

        /// <summary>
        /// Depolama departmanına ürün stoğunu düşüren kuyruğa kayıt atan sınıf
        /// </summary>
        private readonly DescendProductStockPublisher _descendProductStockPublisher;

        /// <summary>
        /// Ürün işlemleri iş mantığı sınıfı
        /// </summary>
        /// <param name="mapper">Mapping işlemleri için mapper nesnesi</param>
        /// <param name="unitOfWork">Veritabanı iş birimi nesnesi</param>
        /// <param name="translationProvider">Dil çeviri sağlayıcısı sınıf nesnesi</param>
        /// <param name="redisCacheDataProvider">Rediste tutulan önbellek yönetimini sağlayan sınıf nesnesi</param>
        /// <param name="transactionRepository">İşlem tablosu için repository sınıfı nesnesi</param>
        /// <param name="transactionItemRepository">İşlem öğesi tablosu için repository sınıfı nesnesi</param>
        /// <param name="productRepository">Ürünler repository sınıfı nesnesi</param>
        /// <param name="productDependencyRepository">Ürün bağımlılıkları repository sınıfı</param>
        /// <param name="storageCommunicator">Stok servisi iletişim sağlayıcı sınıf</param>
        /// <param name="createProductRequestPublisher">Satınalma departmanına alınması istenilen ürün talepleri için kayıt açan sınıf nesnesi</param>
        /// <param name="descendProductStockPublisher">Depolama departmanına ürün stoğunu düşüren kuyruğa kayıt atan sınıf</param>
        public ProductionService(
            IMapper mapper,
            IUnitOfWork<ProductionContext> unitOfWork,
            TranslationProvider translationProvider,
            RedisCacheDataProvider redisCacheDataProvider,
            TransactionRepository transactionRepository,
            TransactionItemRepository transactionItemRepository,
            ProductRepository productRepository,
            ProductDependencyRepository productDependencyRepository,
            StorageCommunicator storageCommunicator,
            CreateProductRequestPublisher createProductRequestPublisher,
            DescendProductStockPublisher descendProductStockPublisher)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _translationProvider = translationProvider;
            _redisCacheDataProvider = redisCacheDataProvider;

            _transactionRepository = transactionRepository;
            _transactionItemRepository = transactionItemRepository;
            _productRepository = productRepository;
            _productDependencyRepository = productDependencyRepository;
            _storageCommunicator = storageCommunicator;
            _createProductRequestPublisher = createProductRequestPublisher;
            _descendProductStockPublisher = descendProductStockPublisher;
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
                    case ProductRepository.TABLE_NAME:
                        if (rollbackItem.RollbackType == RollbackType.Delete)
                        {
                            await _productRepository.DeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Insert)
                        {
                            await _productRepository.UnDeleteAsync((int)rollbackItem.Identity, cancellationTokenSource);
                        }
                        else if (rollbackItem.RollbackType == RollbackType.Update)
                        {
                            await _productRepository.SetAsync((int)rollbackItem.Identity, rollbackItem.Name, rollbackItem.OldValue, cancellationTokenSource);
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
            await _productRepository.DisposeAsync();
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

        /// <summary>
        /// Ürün üretimi işlemini başlatır
        /// </summary>
        /// <param name="produceModel">Üretilecek ürün modeli nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> ProduceProductAsync(ProduceModel produceModel, CancellationTokenSource cancellationTokenSource)
        {
            ProductEntity product = await _productRepository.GetAsync(produceModel.ProductId, cancellationTokenSource);

            if (product != null)
            {
                ProductionEntity production = new ProductionEntity();

                production.ProductId = product.Id;
                production.ReferenceNumber = produceModel.ReferenceNumber;
                production.DepartmentId = produceModel.DepartmentId;

                List<ProductDependencyEntity> productDependencies =
                    await _productDependencyRepository.GetAsQueryable().Where(x => x.ProductId == product.Id).ToListAsync();

                foreach (ProductDependencyEntity dependedProduct in productDependencies)
                {
                    ServiceResultModel<StockModel> stocksServiceResult = await _storageCommunicator.GetStockAsync(dependedProduct.DependedProductId, cancellationTokenSource);

                    if (stocksServiceResult.IsSuccess)
                    {
                        if (stocksServiceResult.Data.Amount < dependedProduct.Amount)
                        {
                            _createProductRequestPublisher.AddToBuffer(new ProductRequestModel()
                            {
                                Amount = dependedProduct.Amount,
                                ProductId = dependedProduct.DependedProductId,
                                ReferenceNumber = production.Id
                            });

                            production.StatusId = (int)ProductionStatus.WaitingDependency;
                        }
                        else
                        {
                            _descendProductStockPublisher.AddToBuffer(new ProductStockModel()
                            {
                                Amount = dependedProduct.Amount,
                                ProductId = dependedProduct.DependedProductId
                            });
                        }
                    }
                    else
                    {
                        throw new CallException(
                                message: stocksServiceResult.ErrorModel.Description,
                                endpoint:
                                !string.IsNullOrEmpty(stocksServiceResult.SourceApiService)
                                ?
                                stocksServiceResult.SourceApiService
                                :
                                $"{ApiServiceName}).{nameof(ProductionService)}.{nameof(ProduceProductAsync)}",
                                error: stocksServiceResult.ErrorModel,
                                validation: stocksServiceResult.Validation);
                    }
                }

                await _unitOfWork.SaveAsync(cancellationTokenSource);

                await _createProductRequestPublisher.PublishBufferAsync(cancellationTokenSource);
                await _descendProductStockPublisher.PublishBufferAsync(cancellationTokenSource);

                return production.Id;
            }
            else
                throw new Exception("Ürün kaydı bulunamadı");

            // TO DO: ürünün bağımlı olduğu alt ürünleri getir
            // alt bağımlı olduğu ürünlerin stoklarını kontrol et
            // -    yoksa satın alma talebi oluştur
            // üretimi gerçekleştir
            // alt bağımlı ürünlerin stoklarını düşür
            // üretimin tamamlandığına dair satış departmanına referans numarasıyla bilgilendirme yap
            // Satış departmanının ürünü nakletmesini sağla
        }
    }
}
