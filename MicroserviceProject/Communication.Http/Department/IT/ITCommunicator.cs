using Communication.Http.Department.IT.Models;

using Infrastructure.Communication.Broker;
using Infrastructure.Communication.Http.Broker.Models;
using Infrastructure.Routing.Providers;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Communication.Http.Department.IT
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
        /// Servis rotalarına ait endpoint isimlerini sağlayan sınıfın nesnesi
        /// </summary>
        private readonly RouteNameProvider _routeNameProvider;

        /// <summary>
        /// Yetki denetimi destekli servis iletişim sağlayıcı sınıfın nesnesi
        /// </summary>
        private readonly ServiceCommunicator _serviceCommunicator;

        /// <summary>
        /// IT servisi için iletişim kurucu sınıf
        /// </summary>
        /// <param name="routeNameProvider">Servis rotalarına ait endpoint isimlerini sağlayan sınıfın nesnesi</param>
        /// <param name="serviceCommunicator">Yetki denetimi destekli servis iletişim sağlayıcı sınıfın nesnesi</param>
        public ITCommunicator(
            RouteNameProvider routeNameProvider,
            ServiceCommunicator serviceCommunicator)
        {
            _routeNameProvider = routeNameProvider;
            _serviceCommunicator = serviceCommunicator;
        }

        /// <summary>
        /// IT envanterlerini verir
        /// </summary>
        /// <param name="transactionIdentity">Servislerin işlem süreçleri boyunca izleyeceği işlem kimliği</param>
        /// <param name="cancellationTokenSource"></param>
        /// <returns></returns>
        public async Task<ServiceResultModel<List<InventoryModel>>> GetInventoriesAsync(string transactionIdentity, CancellationTokenSource cancellationTokenSource)
        {
            ServiceResultModel<List<InventoryModel>> serviceResult =
                    await
                    _serviceCommunicator.Call<List<InventoryModel>>(
                            serviceName: _routeNameProvider.IT_GetInventories,
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
        /// Yeni çalışanlar için IT tarafından varsayılan envanterleri verir
        /// </summary>
        /// <param name="transactionIdentity">Servislerin işlem süreçleri boyunca izleyeceği işlem kimliği</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<List<InventoryModel>>> GetInventoriesForNewWorkerAsync(string transactionIdentity, CancellationTokenSource cancellationTokenSource)
        {
            ServiceResultModel<List<InventoryModel>> defaultInventoriesServiceResult =
                    await _serviceCommunicator.Call<List<InventoryModel>>(
                        serviceName: _routeNameProvider.IT_GetInventoriesForNewWorker,
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
        /// <param name="workerModel">Çalışan modeli</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<int>> AssignInventoryToWorkerAsync(WorkerModel workerModel, CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call<int>(
                serviceName: _routeNameProvider.IT_AssignInventoryToWorker,
                postData: workerModel,
                queryParameters: null,
                headers: null,
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Bilgi teknolojileri departmanının bekleyen satın alımları için bilgilendirme yapar
        /// </summary>
        /// <param name="inventoryRequestModel">Satın alım modeli</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<int>> InformInventoryRequestAsync(InventoryRequestModel inventoryRequestModel, CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call<int>(
                serviceName: _routeNameProvider.IT_InformInventoryRequest,
                postData: inventoryRequestModel,
                queryParameters: null,
                headers: null,
                cancellationTokenSource: cancellationTokenSource);
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
                    _routeNameProvider.Dispose();
                    _serviceCommunicator.Dispose();
                }

                disposed = true;
            }
        }
    }
}
