using Infrastructure.Communication.Http.Broker;

using Services.Communication.Http.Broker.Abstract;

namespace Services.Communication.Http.Broker.Mock
{
    public class DefaultCommunicatorProvider
    {
        public static ICommunicator GetDefaultCommunicator(HttpGetCaller httpGetCaller, HttpPostCaller httpPostCaller)
        {
            return new DefaultCommunicator(httpGetCaller, httpPostCaller);
        }
    }
}
