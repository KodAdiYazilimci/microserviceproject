using Infrastructure.Logging.Model;

namespace Services.Logging.Aspect
{
    public class RuntimeLogModel : BaseLogModel
    {
        public string MethodName { get; set; } = string.Empty;
        public string ParametersAsJson { get; set; } = string.Empty;
        public string ResultAsJson { get; set; } = string.Empty;
    }
}
