using Infrastructure.Caching.Abstraction;
using Infrastructure.Security.Authentication.Abstract;

using Services.Communication.Http.Broker.Abstract;
using Services.Communication.Http.Broker.Authorization.Abstract;
using Services.Communication.WebSockets.Broker.Abstract;

namespace Services.Communication.WebSockets.Broker.Mock
{
    public class SocketCommunicatorProvider
    {
        public static ISocketCommunicator GetSocketCommunicator(
            ICommunicator communicator,
            IAuthorizationCommunicator authorizationCommunicator,
            IInMemoryCacheDataProvider inMemoryCacheDataProvider,
            ICredentialProvider credentialProvider)
        {
            return new SocketCommunicator(
                communicator,
                authorizationCommunicator,
                inMemoryCacheDataProvider,
                credentialProvider);
        }
    }
}
