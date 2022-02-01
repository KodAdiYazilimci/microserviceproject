using Infrastructure.Runtime.Defintion;
using Infrastructure.Runtime.Timing;

namespace Services.Logging.Aspect.Attributes
{
    /// <summary>
    /// Çalışma öncesi loglayan method özniteliği sınıfı
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class LogBeforeRuntimeAttr : Attribute, IExecutionTime, ICustomName
    {
        /// <summary>
        /// Methodun özel ismi
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Loglamanın çalışma sırası
        /// </summary>
        public ExecutionType ExecutionType => ExecutionType.Before;

        /// <summary>
        /// Çalışma öncesi loglayan method özniteliği sınıfı
        /// </summary>
        public LogBeforeRuntimeAttr()
        {

        }

        /// <summary>
        /// Çalışma öncesi loglayan method özniteliği sınıfı
        /// </summary>
        /// <param name="name">Methodun özel ismi</param>
        public LogBeforeRuntimeAttr(string name)
        {
            Name = name;
        }
    }
}
