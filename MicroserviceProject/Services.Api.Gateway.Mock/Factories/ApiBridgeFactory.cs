using Services.Api.Gateway.Util.Communication;

namespace Services.Api.Gateway.Mock.Factories
{
    public class ApiBridgeFactory
    {
        private static ApiBridge apiBridge = null;

        public static ApiBridge Instance
        {
            get
            {
                if (apiBridge == null)
                {
                    apiBridge = new ApiBridge();
                }

                apiBridge.TransactionIdentity = new Random().Next(int.MinValue, int.MaxValue).ToString();

                return apiBridge;
            }
        }
    }
}
