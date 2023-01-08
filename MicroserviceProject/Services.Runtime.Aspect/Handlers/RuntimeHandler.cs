using Infrastructure.Communication.Http.Wrapper;
using Infrastructure.Runtime.Handlers;
using Infrastructure.Runtime.Util;

using Newtonsoft.Json;

using Services.Logging.Aspect.Attributes;

using System.Reflection;

namespace Services.Logging.Aspect.Handlers
{
    /// <summary>
    /// Çalışma zamanı denetimi sağlayan sınıf
    /// </summary>
    public class RuntimeHandler : AspectRuntimeHandlerBase
    {
        /// <summary>
        /// Çalışma zamanı loglayıcı sınıf nesnesi
        /// </summary>
        private readonly RuntimeLogger _runtimeLogger;

        /// <summary>
        /// Çalışma zamanı denetimi sağlayan sınıf
        /// </summary>
        /// <param name="runtimeLogger">Çalışma zamanı loglayıcı sınıf nesnesi</param>
        public RuntimeHandler(RuntimeLogger runtimeLogger)
        {
            _runtimeLogger = runtimeLogger;
        }

        /// <summary>
        /// Method çalıştırma sonrasında çağrılan method
        /// </summary>
        /// <param name="instance">Methodun sınıf örneği</param>
        /// <param name="methodInfo">Çalıştırılacak hedef methodun bilgisi</param>
        /// <param name="methodExecutionAttr">Çalıştırılan methoda atanmış öznitelik</param>
        /// <param name="executionResult">Çalıştırılma sonrası hedef methodun dönüş değeri</param>
        /// <param name="parameters">Çalıştırılan methoda verilen parametreler</param>
        public override void HandleAfterInvoke(object instance, MethodInfo methodInfo, Type methodExecutionAttr, object executionResult, params object[] parameters)
        {
            if (methodExecutionAttr == typeof(LogAfterRuntimeAttr))
            {
                string methodName = Resolver.GetMethodName<LogAfterRuntimeAttr>(methodInfo, methodExecutionAttr);

                Task logTask = _runtimeLogger.LogAsync(new RuntimeLogModel()
                {
                    MethodName = methodName,
                    ResultAsJson = JsonConvert.SerializeObject(executionResult, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore}),
                    ParametersAsJson = JsonConvert.SerializeObject(parameters, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                    Date = DateTime.UtcNow,
                    ApplicationName = instance is BaseService ? (instance as BaseService).ApiServiceName : String.Empty,
                    MachineName = Environment.MachineName,
                }, new CancellationTokenSource());

                logTask.Wait();
            }
        }

        /// <summary>
        /// Method çalıştırma öncesinde çağrılacak method
        /// </summary>
        /// <param name="instance">Methodun sınıf örneği</param>
        /// <param name="methodInfo">Çalıştırılacak hedef methodun bilgisi</param>
        /// <param name="methodExecutionAttr">Çalıştırılacak methoda atanmış öznitelik</param>
        /// <param name="parameters">Çalıştırılacak methoda verilen parametreler</param>
        public override void HandleBeforeInvoke(object instance, MethodInfo methodInfo, Type methodExecutionAttr, params object[] parameters)
        {
            if (methodExecutionAttr == typeof(LogBeforeRuntimeAttr))
            {
                string methodName = Resolver.GetMethodName<LogBeforeRuntimeAttr>(methodInfo, methodExecutionAttr);

                Task logTask = _runtimeLogger.LogAsync(new RuntimeLogModel()
                {
                    MethodName = methodName,
                    ParametersAsJson = JsonConvert.SerializeObject(parameters),
                    Date = DateTime.UtcNow,
                    ApplicationName = instance is BaseService ? (instance as BaseService).ApiServiceName : String.Empty,
                    MachineName = Environment.MachineName,
                }, new CancellationTokenSource());

                logTask.Wait();
            }
        }
    }
}
