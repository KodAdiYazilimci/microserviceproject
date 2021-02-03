using MicroserviceProject.Model.Logging;

using System.Threading.Tasks;

namespace MicroserviceProject.Infrastructure.Logging.Abstraction
{
    /// <summary>
    /// Loglayıcı sınıfların arayüzü
    /// </summary>
    /// <typeparam name="TModel">Yazılacak logun tipi</typeparam>
    public interface ILogger<TModel> where TModel : BaseLogModel, new()
    {
        /// <summary>
        /// Log yazar
        /// </summary>
        /// <param name="model">Yazılacak logun modeli</param>
        Task LogAsync(TModel model);
    }
}
