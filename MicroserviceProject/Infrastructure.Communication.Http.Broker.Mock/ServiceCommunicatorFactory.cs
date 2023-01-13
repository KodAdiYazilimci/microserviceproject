﻿using Infrastructure.Caching.InMemory;
using Infrastructure.Routing.Persistence.Repositories.Sql;
using Infrastructure.Security.Authentication.Providers;

namespace Infrastructure.Communication.Http.Broker.Mock
{
    /// <summary>
    /// Servis iletişim sağlayıcısını taklit eden sınıf
    /// </summary>
    public class ServiceCommunicatorFactory
    {
        /// <summary>
        /// Servis iletişim sağlayıcısı
        /// </summary>
        private static ServiceCommunicator serviceCommunicator = null;

        /// <summary>
        /// Servis iletişim sağlayıcısını verir
        /// </summary>
        /// <param name="cacheProvider">Önbellek nesnesi</param>
        /// <param name="credentialProvider">Kullanıcı bilgi sağlayıcısının nesnesi</param>
        /// <param name="serviceRouteRepository">Servis rota repository sınıfı nesnesi</param>
        /// <returns></returns>
        public static ServiceCommunicator GetServiceCommunicator(
            InMemoryCacheDataProvider cacheProvider,
            CredentialProvider credentialProvider,
            ServiceRouteRepository serviceRouteRepository,
            ServiceCaller serviceCaller)
        {
            if (serviceCommunicator == null)
            {
                serviceCommunicator = new ServiceCommunicator(cacheProvider, credentialProvider, serviceRouteRepository, serviceCaller);
            }

            return serviceCommunicator;
        }
    }
}
