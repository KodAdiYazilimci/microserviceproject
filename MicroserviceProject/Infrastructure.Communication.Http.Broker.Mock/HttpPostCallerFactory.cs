namespace Infrastructure.Communication.Http.Broker.Mock
{
    public class HttpPostCallerFactory
    {
        private static HttpPostCaller httpPostCaller = null;

        public static HttpPostCaller Instance
        {
            get
            {
                return new HttpPostCaller();
            }
        }
    }
}
