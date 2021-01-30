using MicroserviceProject.Common.Model.Communication.Errors;
using MicroserviceProject.Common.Model.Communication.Validations;

namespace MicroserviceProject.Common.Model.Communication.Basics
{
    public class ServiceResult
    {
        public bool IsSuccess { get; set; }
        public Error Error { get; set; }
        public Validation Validation { get; set; }
    }
}
