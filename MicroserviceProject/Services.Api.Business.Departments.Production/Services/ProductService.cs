using AutoMapper;

using Infrastructure.Caching.Abstraction;
using Infrastructure.Communication.Http.Wrapper;
using Infrastructure.Communication.Http.Wrapper.Disposing;
using Infrastructure.Localization.Translation.Provider;
using Infrastructure.Transaction.Recovery;
using Infrastructure.Transaction.UnitOfWork.EntityFramework;

using Services.Api.Business.Departments.Production.Configuration.Persistence;
using Services.Api.Business.Departments.Production.Entities.EntityFramework;
using Services.Api.Business.Departments.Production.Repositories.EntityFramework;
using Services.Communication.Http.Broker.Department.Production.Models;
using Services.Logging.Aspect.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Production.Services
{
    /// <summary>
    /// Ürün işlemleri iş mantığı sınıfı
    /// </summary>
    public class ProductService : BaseService, IRollbackableAsync, IAsyncDisposable, IDisposableInjectionsAsync
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// İçerisinde çalışılan servisin adı
        /// </summary>
        public override string ServiceName => "Services.Api.Business.Departments.Production.Services.ProductService";

        /// <summary>
        /// Önbelleğe alınan ürünlerin önbellekteki adı
        /// </summary>
        private const string CACHED_PRODUCTS_KEY = "Services.Api.Business.Departments.Production.Products";

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
        /// Müşteriler repository sınıfı
        /// </summary>
        private readonly ProductRepository _productRepository;

        /// <summary>
        /// Dil çeviri sağlayıcısı sınıf
        /// </summary>
        private readonly TranslationProvider _translationProvider;

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
        public ProductService(
            IMapper mapper,
            IUnitOfWork<ProductionContext> unitOfWork,
            TranslationProvider translationProvider,
            IDistrubutedCacheProvider redisCacheDataProvider,
            TransactionRepository transactionRepository,
            TransactionItemRepository transactionItemRepository,
            ProductRepository productRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _translationProvider = translationProvider;
            _redisCacheDataProvider = redisCacheDataProvider;

            _transactionRepository = transactionRepository;
            _transactionItemRepository = transactionItemRepository;
            _productRepository = productRepository;
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

        [LogBeforeRuntimeAttr(nameof(GetProductsAsync))]
        [LogAfterRuntimeAttr(nameof(GetProductsAsync))]
        public async Task<List<ProductModel>> GetProductsAsync(CancellationTokenSource cancellationTokenSource)
        {
            if (_redisCacheDataProvider.TryGetValue(CACHED_PRODUCTS_KEY, out List<ProductModel> cachedProducts)
                 &&
                 cachedProducts != null && cachedProducts.Any())
            {
                return cachedProducts;
            }

            List<ProductEntity> products = await _productRepository.GetListAsync(cancellationTokenSource);

            List<ProductModel> mappedProducts = _mapper.Map<List<ProductEntity>, List<ProductModel>>(products);

            _redisCacheDataProvider.Set(CACHED_PRODUCTS_KEY, mappedProducts);

            return mappedProducts;
        }

        [LogBeforeRuntimeAttr(nameof(CreateProductAsync))]
        [LogAfterRuntimeAttr(nameof(CreateProductAsync))]
        public async Task<int> CreateProductAsync(ProductModel productModel, CancellationTokenSource cancellationTokenSource)
        {
            ProductEntity mappedProductEntity = _mapper.Map<ProductModel, ProductEntity>(productModel);

            await _productRepository.CreateAsync(mappedProductEntity, cancellationTokenSource);

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
                            Identity = mappedProductEntity.Id,
                            DataSet = ProductRepository.TABLE_NAME,
                            RollbackType = RollbackType.Delete
                        }
                    }
                },
                cancellationTokenSource: cancellationTokenSource);

            await _unitOfWork.SaveAsync(cancellationTokenSource);

            productModel.ProductId = mappedProductEntity.Id;

            if (_redisCacheDataProvider.TryGetValue(CACHED_PRODUCTS_KEY, out List<ProductModel> cachedProducts) && cachedProducts != null)
            {
                cachedProducts.Add(productModel);

                _redisCacheDataProvider.Set(CACHED_PRODUCTS_KEY, cachedProducts);
            }

            return mappedProductEntity.Id;
        }
    }
}
