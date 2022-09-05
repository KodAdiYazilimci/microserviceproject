using Infrastructure.Communication.Http.Broker;
using Infrastructure.Communication.Http.Models;

using Services.Communication.Http.Broker.Department.Storage.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Storage.Models;

namespace Services.Communication.Http.Broker.Department.Storage
{
    /// <summary>
    /// Stok servisi için iletişim kurucu sınıf
    /// </summary>
    public class StorageCommunicator : IDisposable
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
        /// Stok servisi için iletişim kurucu sınıf
        /// </summary>
        /// <param name="serviceCommunicator">Yetki denetimi destekli servis iletişim sağlayıcı sınıfın nesnesi</param>
        public StorageCommunicator(
            ServiceCommunicator serviceCommunicator)
        {
            _serviceCommunicator = serviceCommunicator;
        }

        /// <summary>
        /// Stok departmanındaki bir ürünün stok değerini verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<StockModel>> GetStockAsync(
            int productId,
            CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call<StockModel>(
                serviceName: "storage.stock.getstock",
                postData: null,
                queryParameters: new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("productId", productId.ToString()) },
                headers: null,
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Stok departmanındaki bir ürünün stok değerini azaltır
        /// </summary>
        /// <param name="descendProductStockCommandRequest">Stoğu düşürecek model</param>
        /// <param name="transactionIdentity">Servislerin işlem süreçleri boyunca izleyeceği işlem kimliği</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel> DescendStockAsync(
            DescendProductStockCommandRequest descendProductStockCommandRequest,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call(
                serviceName: "storage.stock.descendproductstock",
                postData: descendProductStockCommandRequest,
                queryParameters: null,
                headers: new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("TransactionIdentity", transactionIdentity)
                },
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Stok departmanına yeni bir stok kaydı oluşturur
        /// </summary>
        /// <param name="createStockCommandRequest">Stok modeli</param>
        /// <param name="transactionIdentity">Servislerin işlem süreçleri boyunca izleyeceği işlem kimliği</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel> CreateStockAsync(
            CreateStockCommandRequest createStockCommandRequest,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call(
                serviceName: "storage.stock.createstock",
                postData: createStockCommandRequest,
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
                    serviceName: "storage.identity.removesessionifexistsincache",
                    postData: null,
                    queryParameters: new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("tokenKey",tokenKey)
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
