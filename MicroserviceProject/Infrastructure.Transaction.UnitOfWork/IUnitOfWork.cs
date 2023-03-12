namespace Infrastructure.Transaction.UnitOfWork
{
    public interface IUnitOfWork
    {

        /// <summary>
        /// Veritabanı işlem bütünlüğünü çalıştırır
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        Task SaveAsync(CancellationTokenSource cancellationTokenSource);
    }
}