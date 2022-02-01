namespace Infrastructure.Runtime.Attributes
{
    /// <summary>
    /// Field çalışma özniteliği
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class FieldExecutionAttr : Attribute
    {
        /// <summary>
        /// Field adı
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Field çalışma özniteliği
        /// </summary>
        /// <param name="fieldName">Field adı</param>
        public FieldExecutionAttr(string fieldName)
        {
            FieldName = fieldName;
        }
    }
}
