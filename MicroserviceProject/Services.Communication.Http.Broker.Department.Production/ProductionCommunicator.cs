using Infrastructure.Communication.Http.Broker;
using Infrastructure.Communication.Http.Models;

using Services.Communication.Http.Broker.Department.Production.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Production.Models;

namespace Services.Communication.Http.Broker.Department.Production
{
    /// <summary>
    /// Üretim servisi için iletişim kurucu sınıf
    /// </summary>
    public class ProductionCommunicator : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Yetki denetimi destekli servis iletişim sağlayıcı sınıfın nesnesi
        /// </summary>
        private readonly ServiceCommunicator _serviceCommunicator;

        /// <summary>
        /// Üretim servisi için iletişim kurucu sınıf
        /// </summary>
        /// <param name="serviceCommunicator">Yetki denetimi destekli servis iletişim sağlayıcı sınıfın nesnesi</param>
        public ProductionCommunicator(
            ServiceCommunicator serviceCommunicator)
        {
            _serviceCommunicator = serviceCommunicator;
        }

        /// <summary>
        /// Üretim departmanındaki ürünlerin listesini verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<List<ProductModel>>> GetProductsAsync(
            CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call<List<ProductModel>>(
                serviceName: "production.product.getproducts",
                postData: null,
                queryParameters: null,
                headers: null,
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Üretim departmanına ürün üretme talebi iletir
        /// </summary>
        /// <param name="request">Üretilecek ürünün talep modeli</param>
        /// <param name="transactionIdentity">Servislerin işlem süreçleri boyunca izleyeceği işlem kimliği</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel> ProduceProductAsync(
            ProduceProductCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call(
                serviceName: "production.production.produceproduct",
                postData: request,
                queryParameters: null,
                headers: new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("TransactionIdentity", transactionIdentity)
                },
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Üretim departmanına yeni ürün kaydı oluşturur
        /// </summary>
        /// <param name="request">Ürün modeli</param>
        /// <param name="transactionIdentity">Servislerin işlem süreçleri boyunca izleyeceği işlem kimliği</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel> CreateProductAsync(
            CreateProductCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call(
                serviceName: "production.product.createproduct",
                postData: request,
                queryParameters: null,
                headers: new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("TransactionIdentity", transactionIdentity)
                },
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// İptal edilmesi istenilen bir session ın düşürülmesi talebini iletir
        /// </summary>
        /// <param name="tokenKey">Düşürülecek session a ait token</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel> RemoveSessionIfExistsInCacheAsync(
            string tokenKey,
            CancellationTokenSource cancellationTokenSource)
        {
            ServiceResultModel serviceResult =
                await _serviceCommunicator.Call(
                    serviceName: "production.identity.removesessionifexistsincache",
                    postData: null,
                    queryParameters: new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("tokenKey",tokenKey)
                    },
                    headers: null,
                    cancellationTokenSource);

            return serviceResult;
        }

        public async Task<ServiceResultModel> ReEvaluateProduceProductAsync(
            int referenceNumber,
            CancellationTokenSource cancellationTokenSource)
        {
            ServiceResultModel serviceResult =
                await _serviceCommunicator.Call(
                    serviceName: "production.production.reevaluateproduceproduct",
                    postData: null,
                    queryParameters: new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("referenceNumber",referenceNumber.ToString())
                    },
                    headers: null,
                    cancellationTokenSource);

            return serviceResult;
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    _serviceCommunicator.Dispose();
                }

                disposed = true;
            }
        }
    }
}
