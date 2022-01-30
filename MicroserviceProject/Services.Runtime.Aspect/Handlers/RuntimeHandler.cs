using Infrastructure.Runtime.Defintion;
using Infrastructure.Runtime.Handlers;
using Infrastructure.Runtime.Util;

using Newtonsoft.Json;

using Services.Logging.Aspect.Attributes;

using System.Reflection;

namespace Services.Logging.Aspect.Handlers
{
    public class RuntimeHandler : AspectRuntimeHandlerBase
    {
        private readonly RuntimeLogger _runtimeLogger;

        public RuntimeHandler(RuntimeLogger runtimeLogger)
        {
            _runtimeLogger = runtimeLogger;
        }

        public override void HandleAfterInvoke(object instance, MethodInfo methodInfo, Type methodExecutionAttr, object executionResult, params object[] passedParameters)
        {
            if (methodExecutionAttr == typeof(LogAfterRuntimeAttr))
            {
                string methodName = Resolver.GetMethodName<LogAfterRuntimeAttr>(methodInfo, methodExecutionAttr);

                Task logTask = _runtimeLogger.LogAsync(new RuntimeLogModel()
                {
                    MethodName = methodName,
                    ResultAsJson = JsonConvert.SerializeObject(executionResult),
                    ParametersAsJson = JsonConvert.SerializeObject(passedParameters),
                    Date = DateTime.Now
                }, new CancellationTokenSource());

                logTask.Wait();
            }
        }

        public override void HandleBeforeInvoke(object instance, MethodInfo methodInfo, Type methodExecutionAttr, params object[] passedParameters)
        {
            if (methodExecutionAttr == typeof(LogBeforeRuntimeAttr))
            {
                string methodName = Resolver.GetMethodName<LogBeforeRuntimeAttr>(methodInfo, methodExecutionAttr);

                Task logTask = _runtimeLogger.LogAsync(new RuntimeLogModel()
                {
                    MethodName = methodName,
                    ParametersAsJson = JsonConvert.SerializeObject(passedParameters),
                    Date = DateTime.Now
                }, new CancellationTokenSource());

                logTask.Wait();
            }
        }
    }
}
