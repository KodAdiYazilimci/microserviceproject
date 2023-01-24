using Infrastructure.Communication.Http.Broker;

namespace Services.Communication.Http.Broker.Authorization.Mock
{
    public class AuthorizationCommunicatorProvider
    {
        private static AuthorizationCommunicator authorizationCommunicator;

        public static AuthorizationCommunicator GetAuthorizationCommunicator(ServiceCommunicator serviceCommunicator)
        {
            if (authorizationCommunicator == null)
            {
                authorizationCommunicator = new AuthorizationCommunicator(serviceCommunicator);
            }

            return authorizationCommunicator;
        }
    }
}
