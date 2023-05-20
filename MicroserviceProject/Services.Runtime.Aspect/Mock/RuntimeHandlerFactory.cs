using Services.Logging.Aspect;
using Services.Logging.Aspect.Handlers;

namespace Services.Runtime.Aspect.Mock
{
    public static class RuntimeHandlerFactory
    {
        public static RuntimeHandler GetInstance(RuntimeLogger runtimeLogger)
        {
            return new RuntimeHandler(runtimeLogger);
        }
    }
}
