using Infrastructure.Communication.Http.Broker;
using Infrastructure.Communication.Http.Models;

using Services.Communication.Http.Broker.Department.IT.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.IT.Models;

namespace Services.Communication.Http.Broker.Department.IT
{
    /// <summary>
    /// IT servisi için iletişim kurucu sınıf
    /// </summary>
    public class ITCommunicator : IDisposable
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
        /// IT servisi için iletişim kurucu sınıf
        /// </summary>
        /// <param name="serviceCommunicator">Yetki denetimi destekli servis iletişim sağlayıcı sınıfın nesnesi</param>
        public ITCommunicator(
            ServiceCommunicator serviceCommunicator)
        {
            _serviceCommunicator = serviceCommunicator;
        }

        /// <summary>
        /// IT envanterlerini verir
        /// </summary>
        /// <param name="transactionIdentity">Servislerin işlem süreçleri boyunca izleyeceği işlem kimliği</param>
        /// <param name="cancellationTokenSource"></param>
        /// <returns></returns>
        public async Task<ServiceResultModel<List<InventoryModel>>> GetInventoriesAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            ServiceResultModel<List<InventoryModel>> serviceResult =
                    await
                    _serviceCommunicator.Call<List<InventoryModel>>(
                            serviceName: "it.inventory.getinventories",
                            postData: null,
                            queryParameters: null,
                            headers: new List<KeyValuePair<string, string>>()
                            {
                                new KeyValuePair<string, string>("TransactionIdentity", transactionIdentity)
                            },
                            cancellationTokenSource: cancellationTokenSource);

            return serviceResult;
        }

        /// <summary>
        /// IT envanteri oluşturur
        /// </summary>
        /// <<param name="request">Envanter modeli</param>
        /// <param name="transactionIdentity">Servislerin işlem süreçleri boyunca izleyeceği işlem kimliği</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        public async Task<ServiceResultModel> CreateInventoryAsync(
            CreateInventoryCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call(
                serviceName: "it.inventory.createinventory",
                postData: request,
                queryParameters: null,
                headers: new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("TransactionIdentity", transactionIdentity)
                },
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Yeni çalışanlar için IT tarafından varsayılan envanterleri verir
        /// </summary>
        /// <param name="transactionIdentity">Servislerin işlem süreçleri boyunca izleyeceği işlem kimliği</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<List<InventoryModel>>> GetInventoriesForNewWorkerAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            ServiceResultModel<List<InventoryModel>> defaultInventoriesServiceResult =
                    await _serviceCommunicator.Call<List<InventoryModel>>(
                        serviceName: "it.inventory.getinventoriesfornewworker",
                        postData: null,
                        queryParameters: null,
                        headers: new List<KeyValuePair<string, string>>()
                        {
                            new KeyValuePair<string, string>("TransactionIdentity", transactionIdentity)
                        },
                        cancellationTokenSource);

            return defaultInventoriesServiceResult;
        }

        /// <summary>
        /// Çalışana IT envanteri ataması yapar
        /// </summary>
        /// <param name="request">Çalışan modeli</param>
        /// <param name="transactionIdentity">Servislerin işlem süreçleri boyunca izleyeceği işlem kimliği</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel> AssignInventoryToWorkerAsync(
            AssignInventoryToWorkerCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call(
                serviceName: "it.inventory.assigninventorytoworker",
                postData: request,
                queryParameters: null,
                headers: new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("TransactionIdentity", transactionIdentity)
                },
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Yeni çalışan için varsayılan IT envanteri ataması yapar
        /// </summary>
        /// <param name="request">Envanter modeli</param>
        /// <param name="transactionIdentity">Servislerin işlem süreçleri boyunca izleyeceği işlem kimliği</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        public async Task<ServiceResultModel> CreateDefaultInventoryForNewWorkerAsync(
            CreateDefaultInventoryForNewWorkerCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call(
                serviceName: "it.inventory.createdefaultinventoryfornewworker",
                postData: request,
                queryParameters: null,
                headers: new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("TransactionIdentity", transactionIdentity)
                },
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Yeni çalışanlar için IT tarafından varsayılan envanterleri verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        public async Task<ServiceResultModel<List<InventoryModel>>> GetInventoriesForNewWorkerAsync(
            CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call<List<InventoryModel>>(
                serviceName: "it.inventory.getinventoriesfornewworker",
                postData: null,
                queryParameters: null,
                headers: null,
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Bilgi teknolojileri departmanının bekleyen satın alımları için bilgilendirme yapar
        /// </summary>
        /// <param name="request">Satın alım modeli</param>
        /// <param name="transactionIdentity">Servislerin işlem süreçleri boyunca izleyeceği işlem kimliği</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel> InformInventoryRequestAsync(
            InformInventoryRequestCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call(
                serviceName: "it.inventory.informinventoryrequest",
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
                    serviceName: "it.identity.removesessionifexistsincache",
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
