using System;

namespace MicroserviceProject.Model.Logging
{
    public abstract class BaseLogModel
    {
        public string MachineName { get; set; }

        public string ApplicationName { get; set; }

        public string LogText { get; set; }

        public DateTime Date { get; set; }
    }
}
