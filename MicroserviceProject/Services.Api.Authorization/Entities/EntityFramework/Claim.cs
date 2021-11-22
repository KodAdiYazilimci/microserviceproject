namespace Services.Api.Infrastructure.Authorization.Entities.EntityFramework
{
    /// <summary>
    /// Kullanıcı niteliği sınıfı
    /// </summary>
    public class Claim : BaseEntity
    {
        public int ClaimTypeId { get; set; }
        public int UserId { get; set; }

        /// <summary>
        /// Niteliğin değeri
        /// </summary>
        public string Value { get; set; }

        public virtual ClaimType ClaimType { get; set; }
        public virtual User User { get; set; }
    }
}
