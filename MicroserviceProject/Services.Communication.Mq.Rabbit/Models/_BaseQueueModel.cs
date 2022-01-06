namespace Services.Communication.Mq.Rabbit
{
    /// <summary>
    /// Kuyruk modellerinin temel sınıfı
    /// </summary>
    public abstract class BaseQueueModel
    {
        /// <summary>
        /// Kuyruk öğesini oluşturan
        /// </summary>
        public string GeneratedBy { get; set; }

        /// <summary>
        /// Kuyruk öğesinin ait olduğu işlem kimliği
        /// </summary>
        public string TransactionIdentity { get; set; }
    }
}
