namespace Infrastructure.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class FieldExecutionAttr : Attribute
    {
        public string FieldName { get; set; }

        public FieldExecutionAttr(string methodName)
        {
            FieldName = methodName;
        }
    }
}
