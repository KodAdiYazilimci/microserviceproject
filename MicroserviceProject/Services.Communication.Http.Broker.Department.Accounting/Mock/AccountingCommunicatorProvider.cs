using Infrastructure.Caching.InMemory;
using Infrastructure.Communication.Http.Broker;
using Infrastructure.Routing.Providers;
using Infrastructure.Security.Authentication.Providers;

using Services.Communication.Http.Broker.Authorization;

namespace Services.Communication.Http.Broker.Department.Accounting.Mock
{
    public class AccountingCommunicatorProvider
    {
        private static NewAccountingCommunicator accountingCommunicator = null;

        public static NewAccountingCommunicator GetAccountingCommunicator(
            AuthorizationCommunicator authorizationCommunicator,
            InMemoryCacheDataProvider inMemoryCacheDataProvider,
            CredentialProvider credentialProvider,
            HttpGetCaller httpGetCaller,
            HttpPostCaller httpPostCaller,
            RouteProvider routeProvider)
        {
            if (accountingCommunicator == null)
            {
                accountingCommunicator = new NewAccountingCommunicator(
                    authorizationCommunicator,
                    inMemoryCacheDataProvider,
                    credentialProvider,
                    httpGetCaller,
                    httpPostCaller,
                    routeProvider);
            }

            return accountingCommunicator;
        }
    }
}
