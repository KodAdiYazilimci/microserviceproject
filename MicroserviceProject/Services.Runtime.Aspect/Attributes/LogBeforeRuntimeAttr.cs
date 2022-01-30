using Infrastructure.Runtime.Defintion;
using Infrastructure.Runtime.Timing;

namespace Services.Logging.Aspect.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class LogBeforeRuntimeAttr : Attribute, IExecutionTime, ICustomName
    {
        public string Name { get; set; }

        public ExecutionType ExecutionType => ExecutionType.Before;

        public LogBeforeRuntimeAttr()
        {

        }

        public LogBeforeRuntimeAttr(string name)
        {
            Name = name;
        }
    }
}
