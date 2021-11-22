using Communication.Http.Department.Buying.Models;

using Infrastructure.Communication.Broker;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Routing.Providers;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Communication.Http.Department.Buying
{
    /// <summary>
    /// Satınalma servisi için iletişim kurucu sınıf
    /// </summary>
    public class BuyingCommunicator : IDisposable
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
        /// Satınalma servisi için iletişim kurucu sınıf
        /// </summary>
        /// <param name="routeNameProvider">Servis rotalarına ait endpoint isimlerini sağlayan sınıfın nesnesi</param>
        /// <param name="serviceCommunicator">Yetki denetimi destekli servis iletişim sağlayıcı sınıfın nesnesi</param>
        public BuyingCommunicator(
            RouteNameProvider routeNameProvider,
            ServiceCommunicator serviceCommunicator)
        {
            _routeNameProvider = routeNameProvider;
            _serviceCommunicator = serviceCommunicator;
        }

        /// <summary>
        /// Satınalma departmanındaki envanter taleplerini getirir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<List<InventoryRequestModel>>> GetInventoryRequests(CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call<List<InventoryRequestModel>>(
                serviceName: _routeNameProvider.Buying_GetInventoryRequests,
                postData: null,
                queryParameters: null,
                headers: null,
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Satın alınması planlanan envantere ait masrafın sonuçlandırılmasını sağlar
        /// </summary>
        /// <param name="decidedCostModel">Masraf modeli</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<int>> ValidateCostInventoryAsync(DecidedCostModel decidedCostModel, CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call<int>(
                 serviceName: _routeNameProvider.Buying_ValidateCostInventory,
                 postData: decidedCostModel,
                 queryParameters: null,
                 headers: null,
                 cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Satınalma departmanına envanter talebi oluşturur
        /// </summary>
        /// <param name="inventoryRequestModel">Envanter talep modeli</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<int>> CreateInventoryRequestAsync(InventoryRequestModel inventoryRequestModel, CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call<int>(
                serviceName: _routeNameProvider.Buying_CreateInventoryRequest,
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
