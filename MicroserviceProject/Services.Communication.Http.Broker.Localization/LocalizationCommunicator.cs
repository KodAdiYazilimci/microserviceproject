using Infrastructure.Communication.Http.Broker;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker.Localization.Models;

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Communication.Http.Broker.Localization
{
    /// <summary>
    /// Localization servisi için iletişim kurucu sınıf
    /// </summary>
    public class LocalizationCommunicator : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Localization rotalarına ait endpoint isimlerini sağlayan sınıfın nesnesi
        /// </summary>
        private readonly RouteNameProvider _routeNameProvider;

        /// <summary>
        /// Yetki denetimi destekli servis iletişim sağlayıcı sınıfın nesnesi
        /// </summary>
        private readonly ServiceCommunicator _serviceCommunicator;

        /// <summary>
        /// Localization servisi için iletişim kurucu sınıf
        /// </summary>
        /// <param name="routeNameProvider">Localization rotalarına ait endpoint isimlerini sağlayan sınıfın nesnesi</param>
        /// <param name="serviceCommunicator">Yetki denetimi destekli servis iletişim sağlayıcı sınıfın nesnesi</param>
        public LocalizationCommunicator(
            RouteNameProvider routeNameProvider,
            ServiceCommunicator serviceCommunicator)
        {
            _routeNameProvider = routeNameProvider;
            _serviceCommunicator = serviceCommunicator;
        }

        /// <summary>
        /// Dil çevirilerinin listesini verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<TranslationModel>> GetTranslationsAsync(CancellationTokenSource cancellationTokenSource)
        {
            ServiceResultModel<TranslationModel> serviceResultModel =
                   await _serviceCommunicator.Call<TranslationModel>(
                       serviceName: _routeNameProvider.Localization_GetTranslations,
                       postData: null,
                       queryParameters: null,
                       headers: null,
                       cancellationTokenSource: cancellationTokenSource);

            return serviceResultModel;
        }

        public async Task<ServiceResultModel<TranslationModel>> TranslateAsync(string key, string region, List<KeyValuePair<string, string>> parameters, CancellationTokenSource cancellationTokenSource)
        {
            ServiceResultModel<TranslationModel> serviceResultModel =
                await _serviceCommunicator.Call<TranslationModel>(
                    serviceName: _routeNameProvider.Localization_Translate,
                    postData: new TranslationModel()
                    {
                        Key = key,
                        Region = region,
                        Parameters = parameters
                    },
                    queryParameters: null,
                    headers: null,
                    cancellationTokenSource: cancellationTokenSource);

            return serviceResultModel;
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
