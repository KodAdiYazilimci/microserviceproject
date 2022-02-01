namespace Infrastructure.Runtime.Attributes
{
    /// <summary>
    /// Property özniteliği
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyExecutionAttr : Attribute
    {
        /// <summary>
        /// Property adı
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Property özniteliği
        /// </summary>
        /// <param name="propetyName">Property adı</param>
        public PropertyExecutionAttr(string propetyName)
        {
            PropertyName = propetyName;
        }
    }
}
