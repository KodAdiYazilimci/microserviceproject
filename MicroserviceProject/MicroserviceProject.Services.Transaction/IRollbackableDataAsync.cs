using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Transaction
{
    /// <summary>
    /// İşlemin geri alınması için veri setinin uygulayacağı kurallar arayüzü
    /// </summary>
    /// <typeparam name="TIdentity">İşlemin geri dönüş tipi</typeparam>
    public interface IRollbackableDataAsync<TIdentity>
    {
        /// <summary>
        /// İşlemden etkilenerek oluşturulan kaydı siler
        /// </summary>
        /// <param name="id">Silinecek kaydın Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns>TIdentity işlemin geri dönüş tipidir</returns>
        Task<TIdentity> DeleteAsync(TIdentity id, CancellationTokenSource cancellationTokenSource);

        /// <summary>
        /// İşlemden etkinlenerek silinmiş kaydı geri getirir
        /// </summary>
        /// <param name="id">Geri getirilecek kaydın Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns>TIdentity işlemin geri dönüş tipidir</returns>
        Task<TIdentity> UnDeleteAsync(TIdentity id, CancellationTokenSource cancellationTokenSource);

        /// <summary>
        /// İşlemden etkilenerek değiştirilmiş bir kaydı eski haline geri getirir
        /// </summary>
        /// <param name="id">Eski haline geri getirilecek kaydın Id değeri</param>
        /// <param name="name">Kaydın değiştirilecek kısmı</param>
        /// <param name="value">Geri getirilecek değer</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns>TIdentity işlemin geri dönüş tipidir</returns>
        Task<TIdentity> SetAsync(TIdentity id, string name, object value, CancellationTokenSource cancellationTokenSource);
    }
}
