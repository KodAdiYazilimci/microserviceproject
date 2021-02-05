using FluentValidation;

using MicroserviceProject.Infrastructure.Logging.Model;

namespace MicroserviceProject.Services.Infrastructure.Logging.Configuration.Validation.Logging.WriteRequestResponseLog
{
    /// <summary>
    /// Logging/WriteRequestResponseLog Http endpoint için validasyon kuralı
    /// </summary>
    public class RequestResponseLogModelRule : AbstractValidator<RequestResponseLogModel>
    {
        public RequestResponseLogModelRule()
        {
            RuleFor(x => x.ApplicationName).NotEmpty().WithMessage("Uygulama adı boş geçilemez");
            RuleFor(x => x.MachineName).NotEmpty().WithMessage("Makine adı boş geçilemez");
            RuleFor(x => x.Date).NotEmpty().WithMessage("Tarih boş geçilemez");
        }
    }
}
