using Infrastructure.Runtime.Defintion;
using Infrastructure.Runtime.Timing;

namespace Services.Logging.Aspect.Attributes
{
    /// <summary>
    /// Çalışma sonrası loglayan method özniteliği sınıfı
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class LogAfterRuntimeAttr : Attribute, IExecutionTime, ICustomName
    {
        /// <summary>
        /// Methodun özel ismi
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Loglamanın çalışma sırası
        /// </summary>
        public ExecutionType ExecutionType => ExecutionType.After;

        /// <summary>
        /// Çalışma sonrası loglayan method özniteliği sınıfı
        /// </summary>
        public LogAfterRuntimeAttr()
        {

        }

        /// <summary>
        /// Çalışma sonrası loglayan method özniteliği sınıfı
        /// </summary>
        /// <param name="name">Methodun özel ismi</param>
        public LogAfterRuntimeAttr(string name)
        {
            Name = name;
        }
    }
}
