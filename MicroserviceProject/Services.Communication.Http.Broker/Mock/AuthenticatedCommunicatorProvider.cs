using Infrastructure.Communication.Http.Broker;

using Services.Communication.Http.Broker.Abstract;

namespace Services.Communication.Http.Broker.Mock
{
    public class AuthenticatedCommunicatorProvider
    {
        public static ICommunicator GetAuthenticatedCommunicator(HttpGetCaller httpGetCaller, HttpPostCaller httpPostCaller, HttpPutCaller httpPutCaller, HttpDeleteCaller httpDeleteCaller)
        {
            return new AuthenticatedCommunicator(httpGetCaller, httpPostCaller, httpPutCaller, httpDeleteCaller);
        }
    }
}
