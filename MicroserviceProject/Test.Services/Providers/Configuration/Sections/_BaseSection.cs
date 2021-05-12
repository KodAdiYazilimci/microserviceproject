using System.Collections.Generic;

namespace MicroserviceProject.Test.Services.Providers.Configuration.Sections
{
    public class BaseSection
    {
        protected readonly Dictionary<string, string> Data = new Dictionary<string, string>();
        public string this[string key] { get => Data[key]; set { Data[key] = value; } }
        public string Key { get; }
        public string Path { get; }
        public string Value { get; set; }
    }
}
