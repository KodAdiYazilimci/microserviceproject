namespace Services.Communication.Http.Broker.Authorization.Models
{
    /// <summary>
    /// Kullanıcı niteliği sınıfı
    /// </summary>
    public class ClaimModel
    {
        /// <summary>
        /// Niteliğin adı
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Niteliğin değeri
        /// </summary>
        public string Value { get; set; }
    }
}
