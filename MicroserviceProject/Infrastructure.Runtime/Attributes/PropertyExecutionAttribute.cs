namespace Infrastructure.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyExecutionAttr : Attribute
    {
        public string PropertyName { get; set; }

        public PropertyExecutionAttr(string methodName)
        {
            PropertyName = methodName;
        }
    }
}
