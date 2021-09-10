using System.Threading.Tasks;

namespace Infrastructure.Communication.Http.Wrapper.Disposing
{
    /// <summary>
    /// Enjekte edilmiş nesneleri asenkron olarak dispose edecek arayüz
    /// </summary>
    public interface IDisposableInjectionsAsync
    {
        /// <summary>
        /// DI nesneslerini asenkron olarak dispose eder
        /// </summary>
        Task DisposeInjectionsAsync();
    }
}
