namespace Infrastructure.Runtime.Attributes
{
    /// <summary>
    /// Method çalışma özniteliği
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class MethodExecutionAttr : Attribute
    {
        /// <summary>
        /// Method adı
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// Method çalışma özniteliği
        /// </summary>
        /// <param name="methodName">Method adı</param>
        public MethodExecutionAttr(string methodName)
        {
            MethodName = methodName;
        }
    }
}
