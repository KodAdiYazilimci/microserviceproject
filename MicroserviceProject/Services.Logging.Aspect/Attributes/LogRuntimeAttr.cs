namespace Services.Logging.Aspect.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class LogRuntimeAttr : Attribute
    {
        public string Name { get; set; }

        public LogRuntimeAttr(string name)
        {
            Name = name;
        }
    }
}
