using Infrastructure.Runtime.Handlers;

using Newtonsoft.Json;

using Services.Logging.Aspect.Attributes;

namespace Services.Logging.Aspect.Handlers
{
    public class RuntimeLogHandler : AspectRuntimeHandlerBase<LogRuntimeAttr>
    {
        private readonly RuntimeLogger _runtimeLogger;

        public RuntimeLogHandler(RuntimeLogger runtimeLogger)
        {
            _runtimeLogger = runtimeLogger;
        }

        public override void HandleAfterInvoke(LogRuntimeAttr methodExecutionAttr, object executionResult, params object[] passedParameters)
        {
            Task logTask = _runtimeLogger.LogAsync(new RuntimeLogModel()
            {
                MethodName = methodExecutionAttr.Name,
                ResultAsJson = JsonConvert.SerializeObject(executionResult),
                ParametersAsJson = JsonConvert.SerializeObject(passedParameters),
                Date = DateTime.Now
            }, new CancellationTokenSource());

            logTask.Wait();
        }

        public override void HandleBeforeInvoke(LogRuntimeAttr methodExecutionAttr, params object[] passedParameters)
        {
            Task logTask = _runtimeLogger.LogAsync(new RuntimeLogModel()
            {
                MethodName = methodExecutionAttr.Name,
                ParametersAsJson = JsonConvert.SerializeObject(passedParameters),
                Date = DateTime.Now
            }, new CancellationTokenSource());

            logTask.Wait();
        }
    }
}
