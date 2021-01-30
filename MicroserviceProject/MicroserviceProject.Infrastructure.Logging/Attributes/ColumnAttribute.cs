using System;

namespace MicroserviceProject.Infrastructure.Logging.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public string Name { get; set; }

        public ColumnAttribute(string name)
        {
            this.Name = name;
        }
    }
}
