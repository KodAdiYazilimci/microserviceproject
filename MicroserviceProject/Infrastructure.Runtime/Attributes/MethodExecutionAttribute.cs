namespace Infrastructure.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MethodExecutionAttr : Attribute
    {
        public string MethodName { get; set; }

        public MethodExecutionAttr(string methodName)
        {
            MethodName = methodName;
        }
    }
}
