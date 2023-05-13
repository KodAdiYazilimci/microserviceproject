using System.Collections.Generic;

namespace Services.Scheduling.Diagnostics.HealthCheck.Persistence
{
    public class TempData
    {
        public List<Log> Logs { get; set; } = new List<Log>();
    }

    public class Log
    {
        public string LogText { get; set; }
    }
}
