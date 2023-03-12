using AutoMapper;

using Infrastructure.Caching.Abstraction;
using Infrastructure.Communication.Http.Exceptions;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Communication.Http.Wrapper;
using Infrastructure.Communication.Http.Wrapper.Disposing;
using Infrastructure.Localization.Translation.Provider;
using Infrastructure.Transaction.Recovery;
using Infrastructure.Transaction.UnitOfWork.EntityFramework;

using Microsoft.EntityFrameworkCore;

using Services.Api.Business.Departments.Production.Configuration.Persistence;
using Services.Api.Business.Departments.Production.Constants;
using Services.Api.Business.Departments.Production.Entities.EntityFramework;
using Services.Api.Business.Departments.Production.Repositories.EntityFramework;
using Services.Communication.Http.Broker.Department.Production.Models;
using Services.Communication.Http.Broker.Department.Storage.Abstract;
using Services.Communication.Http.Broker.Department.Storage.Models;
using Services.Communication.Mq.Queue.Buying.Models;
using Services.Communication.Mq.Queue.Buying.Rabbit.Publishers;
using Services.Communication.Mq.Queue.Storage.Models;
using Services.Communication.Mq.Queue.Storage.Rabbit.Publishers;
using Services.Logging.Aspect.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Production.Services
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
        public override string ServiceName => "Services.Api.Business.Departments.Production.Services.ProductionService";

        /// <summary>
        /// Servisin ait olduğu api servisinin adı
        /// </summary>
        public override string ApiServiceName => "Services.Api.Business.Departments.Production";

        /// <summary>
        /// Rediste tutulan önbellek yönetimini sağlayan sınıf
        /// </summary>
        private readonly IDistrubutedCacheProvider _redisCacheDataProvider;

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
        /// Üretilen ürünler repository sınıfı
        /// </summary>
        private readonly ProductionRepository _productionRepository;

        /// <summary>
        /// Üretilen ürünler detay repository sınıfı
        /// </summary>
        private readonly ProductionItemRepository _productionItemRepository;

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
        private readonly IStorageCommunicator _storageCommunicator;

        /// <summary>
        /// Satınalma departmanına alınması istenilen ürün talepleri için kayıt açan sınıf
        /// </summary>
        private readonly CreateProductRequestPublisher _createProductRequestPublisher;

        /// <summary>
        /// Depolama departmanına ürün stoğunu düşüren kuyruğa kayıt atan sınıf
        /// </summary>
        private readonly DescendProductStockPublisher _descendProductStockPublisher;

        /// <summary>
        /// Depolama departmanına ürün stoğunu artıran kuyruğa kayıt atan sınıf
        /// </summary>
        private readonly IncreaseProductStockPublisher _increaseProductStockPublisher;

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
        /// <param name="productionRepository">Üretilen ürünler repository sınıfı</param>
        /// <param name="productionItemRepository">Üretilen ürünler detay repository sınıfı</param>
        /// <param name="productDependencyRepository">Ürün bağımlılıkları repository sınıfı</param>
        /// <param name="storageCommunicator">Stok servisi iletişim sağlayıcı sınıf</param>
        /// <param name="createProductRequestPublisher">Satınalma departmanına alınması istenilen ürün talepleri için kayıt açan sınıf nesnesi</param>
        /// <param name="descendProductStockPublisher">Depolama departmanına ürün stoğunu düşüren kuyruğa kayıt atan sınıf</param>
        /// <param name="increaseProductStockPublisher">Depolama departmanına ürün stoğunu artıran kuyruğa kayıt atan sınıf</param>
        public ProductionService(
            IMapper mapper,
            IUnitOfWork<ProductionContext> unitOfWork,
            TranslationProvider translationProvider,
            IDistrubutedCacheProvider redisCacheDataProvider,
            TransactionRepository transactionRepository,
            TransactionItemRepository transactionItemRepository,
            ProductRepository productRepository,
            ProductionRepository productionRepository,
            ProductionItemRepository productionItemRepository,
            ProductDependencyRepository productDependencyRepository,
            IStorageCommunicator storageCommunicator,
            CreateProductRequestPublisher createProductRequestPublisher,
            DescendProductStockPublisher descendProductStockPublisher,
            IncreaseProductStockPublisher increaseProductStockPublisher)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _translationProvider = translationProvider;
            _redisCacheDataProvider = redisCacheDataProvider;

            _transactionRepository = transactionRepository;
            _transactionItemRepository = transactionItemRepository;
            _productRepository = productRepository;
            _productionRepository = productionRepository;
            _productDependencyRepository = productDependencyRepository;
            _storageCommunicator = storageCommunicator;
            _createProductRequestPublisher = createProductRequestPublisher;
            _descendProductStockPublisher = descendProductStockPublisher;
            _increaseProductStockPublisher = increaseProductStockPublisher;
            _productionItemRepository = productionItemRepository;
        }

        /// <summary>
        /// Bir işlemi geri almak için yedekleme noktası oluşturur
        /// </summary>
        /// <param name="rollback">İşlemin yedekleme noktası nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns>TIdentity işlemin geri dönüş tipidir</returns>
        [LogBeforeRuntimeAttr(nameof(CreateCheckpointAsync))]
        [LogAfterRuntimeAttr(nameof(CreateCheckpointAsync))]
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
        [LogBeforeRuntimeAttr(nameof(RollbackTransactionAsync))]
        [LogAfterRuntimeAttr(nameof(RollbackTransactionAsync))]
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
        [LogBeforeRuntimeAttr(nameof(ProduceProductAsync))]
        [LogAfterRuntimeAttr(nameof(ProduceProductAsync))]
        public async Task<int> ProduceProductAsync(ProduceModel produceModel, CancellationTokenSource cancellationTokenSource)
        {
            ProductEntity product = await _productRepository.GetAsync(produceModel.ProductId, cancellationTokenSource);

            if (product != null)
            {
                ProductionEntity production = new ProductionEntity();

                production.ProductId = product.Id;
                production.ReferenceNumber = produceModel.ReferenceNumber;
                production.DepartmentId = produceModel.DepartmentId;
                production.RequestedAmount = produceModel.Amount;
                production.StatusId = (int)ProductionStatus.ReadyToProduce;

                List<ProductDependencyEntity> productDependencies =
                    await _productDependencyRepository.GetAsQueryable().Where(x => x.ProductId == product.Id).ToListAsync();

                foreach (ProductDependencyEntity dependedProduct in productDependencies)
                {
                    ServiceResultModel<StockModel> stocksServiceResult =
                        await _storageCommunicator.GetStockAsync(dependedProduct.DependedProductId, TransactionIdentity, cancellationTokenSource);

                    if (stocksServiceResult.IsSuccess)
                    {
                        ProductionItemEntity productionItem = new ProductionItemEntity();
                        productionItem.DependedProductId = dependedProduct.DependedProductId;
                        productionItem.RequiredAmount = dependedProduct.Amount * produceModel.Amount;

                        if (stocksServiceResult.Data.Amount < dependedProduct.Amount * produceModel.Amount)
                        {
                            productionItem.StatusId = (int)ProductionStatus.WaitingDependency;
                            production.StatusId = (int)ProductionStatus.WaitingDependency;

                            _createProductRequestPublisher.AddToBuffer(new ProductRequestQueueModel()
                            {
                                Amount = dependedProduct.Amount * produceModel.Amount,
                                ProductId = dependedProduct.DependedProductId,
                                ReferenceNumber = production.Id,
                                TransactionIdentity = TransactionIdentity,
                                GeneratedBy = ApiServiceName
                            });
                        }
                        else
                        {
                            productionItem.StatusId = (int)ProductionStatus.ReadyToProduce;

                            _descendProductStockPublisher.AddToBuffer(new ProductStockQueueModel()
                            {
                                Amount = dependedProduct.Amount * produceModel.Amount,
                                ProductId = dependedProduct.DependedProductId,
                                TransactionIdentity = TransactionIdentity,
                                GeneratedBy = ApiServiceName
                            });
                        }

                        productionItem.Production = production;
                        production.ProductionItems.Add(productionItem);

                        await CreateCheckpointAsync(
                            rollback: new RollbackModel()
                            {
                                TransactionType = TransactionType.Insert,
                                TransactionDate = DateTime.UtcNow,
                                TransactionIdentity = TransactionIdentity,
                                RollbackItems = new List<RollbackItemModel>
                                {
                                    new RollbackItemModel()
                                    {
                                        Identity = productionItem.Id,
                                        DataSet = ProductionItemRepository.TABLE_NAME,
                                        RollbackType = RollbackType.Delete
                                    }
                                }
                            },
                            cancellationTokenSource: cancellationTokenSource);
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

                    await CreateCheckpointAsync(
                        rollback: new RollbackModel()
                        {
                            TransactionType = TransactionType.Insert,
                            TransactionDate = DateTime.UtcNow,
                            TransactionIdentity = TransactionIdentity,
                            RollbackItems = new List<RollbackItemModel>
                            {
                                new RollbackItemModel()
                                {
                                    Identity = production.Id,
                                    DataSet = ProductionRepository.TABLE_NAME,
                                    RollbackType = RollbackType.Delete
                                }
                            }
                        },
                        cancellationTokenSource: cancellationTokenSource);

                    await _productionRepository.CreateAsync(production, cancellationTokenSource);
                }

                await _unitOfWork.SaveAsync(cancellationTokenSource);

                if (production.ProductionStatus == ProductionStatus.ReadyToProduce)
                {
                    _increaseProductStockPublisher.AddToBuffer(new ProductStockQueueModel()
                    {
                        ProductId = production.ProductId,
                        Amount = production.RequestedAmount,
                        TransactionIdentity = TransactionIdentity,
                        GeneratedBy = ApiServiceName
                    });
                }

                Task createProductRequestTask = _createProductRequestPublisher.PublishBufferAsync(cancellationTokenSource);
                Task descendProductStockTask = _descendProductStockPublisher.PublishBufferAsync(cancellationTokenSource);
                Task increaseProductStockTask = _increaseProductStockPublisher.PublishBufferAsync(cancellationTokenSource);

                Task.WaitAll(createProductRequestTask, descendProductStockTask, increaseProductStockTask);

                return product.Id;
            }
            else
                throw new Exception("Ürün kaydı bulunamadı");
        }

        /// <summary>
        /// Bileşenlerin stok yetersizliği sebebiyle üretim süreci askıya alınmış ürünün durumunu yeniden değerlendirir
        /// </summary>
        /// <param name="referenceNumber">Askıya alınmış sürecin referans numarası</param>
        /// <param name="cancellationTokenSource">İptal token ı</param>
        /// <returns></returns>
        [LogBeforeRuntimeAttr(nameof(ReEvaluateProduceProductAsync))]
        [LogAfterRuntimeAttr(nameof(ReEvaluateProduceProductAsync))]
        public async Task<int> ReEvaluateProduceProductAsync(int referenceNumber, CancellationTokenSource cancellationTokenSource)
        {
            int executed = 0;

            ProductionEntity production =
                await
                _productionRepository
                .GetAsQueryable()
                .Where(x => x.ReferenceNumber == referenceNumber
                            &&
                            x.StatusId == (int)ProductionStatus.WaitingDependency)
                .FirstOrDefaultAsync(cancellationTokenSource.Token);

            if (production != null)
            {
                production.StatusId = (int)ProductionStatus.ReadyToProduce;

                List<ProductionItemEntity> productionItems =
                    await
                    _productionItemRepository
                    .GetAsQueryable()
                    .Where(x => x.ProductionId == production.Id
                                &&
                                x.StatusId == (int)ProductionStatus.WaitingDependency)
                    .ToListAsync();

                foreach (var productionItem in productionItems)
                {
                    ServiceResultModel<StockModel> stocksServiceResult =
                           await _storageCommunicator.GetStockAsync(productionItem.DependedProductId, TransactionIdentity, cancellationTokenSource);

                    if (stocksServiceResult.IsSuccess)
                    {
                        if (stocksServiceResult.Data.Amount >= productionItem.RequiredAmount)
                        {
                            _descendProductStockPublisher.AddToBuffer(new ProductStockQueueModel()
                            {
                                Amount = productionItem.RequiredAmount,
                                ProductId = productionItem.DependedProductId,
                                TransactionIdentity = TransactionIdentity,
                                GeneratedBy = ApiServiceName
                            });

                            productionItem.StatusId = (int)ProductionStatus.ReadyToProduce;

                            executed++;
                        }
                        else
                        {
                            production.StatusId = (int)ProductionStatus.WaitingDependency;
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
                                $"{ApiServiceName}).{nameof(ProductionService)}.{nameof(ReEvaluateProduceProductAsync)}",
                                error: stocksServiceResult.ErrorModel,
                                validation: stocksServiceResult.Validation);
                    }
                }

                if (production.StatusId == (int)ProductionStatus.ReadyToProduce)
                {
                    _increaseProductStockPublisher.AddToBuffer(new ProductStockQueueModel()
                    {
                        Amount = production.RequestedAmount,
                        ProductId = production.ProductId,
                        TransactionIdentity = TransactionIdentity,
                        GeneratedBy = ApiServiceName
                    });

                    production.StatusId = (int)ProductionStatus.Completed;
                }

                await _unitOfWork.SaveAsync(cancellationTokenSource);

                Task descendProductStockTask = _descendProductStockPublisher.PublishBufferAsync(cancellationTokenSource);
                Task increaseProductStokTask = _increaseProductStockPublisher.PublishBufferAsync(cancellationTokenSource);

                Task.WaitAll(descendProductStockTask, increaseProductStokTask);

                return executed;
            }
            else
                throw new Exception("Üretim kaydı bulunamadı");
        }
    }
}