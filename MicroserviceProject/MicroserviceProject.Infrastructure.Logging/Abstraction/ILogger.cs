using MicroserviceProject.Model.Logging;

namespace MicroserviceProject.Infrastructure.Logging.Abstraction
{
    public interface ILogger<TModel> where TModel : BaseLogModel, new()
    {
        void Log(TModel model);
    }
}
