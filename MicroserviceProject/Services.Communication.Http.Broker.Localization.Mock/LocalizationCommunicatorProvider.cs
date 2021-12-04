using Infrastructure.Communication.Http.Broker;
using Infrastructure.Routing.Providers;

namespace Services.Communication.Http.Broker.Localization.Mock
{
    public class LocalizationCommunicatorProvider
    {
        private static LocalizationCommunicator localizationCommunicator;

        public static LocalizationCommunicator GetLocalizationCommunicator(RouteNameProvider routeNameProvider, ServiceCommunicator serviceCommunicator)
        {
            if (localizationCommunicator == null)
            {
                localizationCommunicator = new LocalizationCommunicator(routeNameProvider, serviceCommunicator);
            }

            return localizationCommunicator;
        }
    }
}
