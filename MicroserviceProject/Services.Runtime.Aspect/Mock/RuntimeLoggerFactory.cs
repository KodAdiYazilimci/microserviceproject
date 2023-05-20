using Microsoft.Extensions.Configuration;

using Services.Logging.Aspect;

namespace Services.Runtime.Aspect.Mock
{
    public static class RuntimeLoggerFactory
    {
        public static RuntimeLogger GetInstance(IConfiguration configuration)
        {
            return new RuntimeLogger(configuration);
        }
    }
}
