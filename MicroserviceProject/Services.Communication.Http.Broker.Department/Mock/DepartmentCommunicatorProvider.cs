using Infrastructure.Caching.Abstraction;
using Infrastructure.Security.Authentication.Abstract;

using Services.Communication.Http.Broker.Abstract;
using Services.Communication.Http.Broker.Authorization.Abstract;
using Services.Communication.Http.Broker.Department.Abstract;

namespace Services.Communication.Http.Broker.Department.Mock
{
    public class DepartmentCommunicatorProvider
    {
        public static IDepartmentCommunicator GetDepartmentCommunicator(
            IAuthorizationCommunicator authorizationCommunicator,
            IInMemoryCacheDataProvider inMemoryCacheDataProvider,
            ICredentialProvider credentialProvider,
            ICommunicator communicator)
        {
            return new DepartmentCommunicator(
                authorizationCommunicator,
                inMemoryCacheDataProvider,
                credentialProvider,
                communicator);
        }
    }
}
