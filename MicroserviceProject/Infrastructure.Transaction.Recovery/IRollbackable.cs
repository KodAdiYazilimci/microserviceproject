namespace MicroserviceProject.Infrastructure.Transaction.Recovery
{
    /// <summary>
    /// Bir veri seti için transaction işlemleri arayüzü
    /// </summary>
    /// <typeparam name="TIdentity">İşlemin geri dönüş tipi</typeparam>
    public interface IRollbackable<TIdentity>
    {
        /// <summary>
        /// İşlem yığınını geri almak için yedekleme noktası oluşturur
        /// </summary>
        /// <param name="rollback">Yedeklemenin modeli</param>
        /// <returns>TIdentity işlemin geri dönüş tipidir</returns>
        TIdentity CreateCheckpoint(RollbackModel rollback);

        /// <summary>
        /// Bir işlemi veri setinden geri alır
        /// </summary>
        /// <param name="rollback">Geri alınacak işlemin yedekleme modeli</param>
        /// <returns>TIdentity işlemin geri dönüş tipidir</returns>
        TIdentity RollbackTransaction(RollbackModel rollback);
    }
}
