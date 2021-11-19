namespace Services.Infrastructure.Authorization.Entities.EntityFramework
{
    public class PolicyRole : BaseEntity
    {
        public int PolicyId { get; set; }
        public int RoleId { get; set; }

        public virtual Policy Policy { get; set; }
        public virtual Role Role { get; set; }
    }
}
