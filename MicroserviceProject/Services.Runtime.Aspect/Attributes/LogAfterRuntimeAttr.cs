using Infrastructure.Runtime.Defintion;
using Infrastructure.Runtime.Timing;

namespace Services.Logging.Aspect.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class LogAfterRuntimeAttr : Attribute, IExecutionTime, ICustomName
    {
        public string Name { get; set; }

        public ExecutionType ExecutionType => ExecutionType.After;

        public LogAfterRuntimeAttr()
        {

        }

        public LogAfterRuntimeAttr(string name)
        {
            Name = name;
        }
    }
}
