namespace Infrastructure.Communication.Mq.Configuration
{
    /// <summary>
    /// Rabbit sunucusunun yapılandırma ayarları
    /// </summary>
    public interface IRabbitConfiguration
    {
        /// <summary>
        /// Rabbit sunucusunun adı
        /// </summary>
        string Host { get; set; }

        /// <summary>
        /// Rabbit sunucusunun kullanıcı adı
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// Rabbit sunucusunun parolası
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Rabbit sunucusunun kuyruk adı
        /// </summary>
        string QueueName { get; set; }
    }
}
