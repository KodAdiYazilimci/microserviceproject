using Microsoft.EntityFrameworkCore;

using Services.Api.Authorization.Entities.EntityFramework;

namespace Services.Api.Authorization.Configuration.Persistence
{
    public class AuthContext : DbContext
    {
        public DbSet<Claim> Claims { get; set; }
        public DbSet<ClaimType> ClaimTypes { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<PolicyRole> PolicyRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public AuthContext() : base()
        {

        }

        public AuthContext(DbContextOptions<AuthContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server=sql-server;DataBase=Microservice_Security_DB;user=sa;password=Srkn_CMR*1987;MultipleActiveResultSets=true;TrustServerCertificate=Yes");

                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.EnableDetailedErrors();
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Claim>().ToTable("CLAIMS");
            modelBuilder.Entity<Claim>().HasKey(x => x.Id);
            modelBuilder.Entity<Claim>().Property(x => x.Id).HasColumnName("ID");
            modelBuilder.Entity<Claim>().Property(x => x.CreateDate).HasColumnName("CREATEDATE");
            modelBuilder.Entity<Claim>().Property(x => x.CreateDate).HasColumnType("DATETIME");
            modelBuilder.Entity<Claim>().Property(x => x.UpdateDate).HasColumnName("UPDATEDATE");
            modelBuilder.Entity<Claim>().Property(x => x.UpdateDate).HasColumnType("DATETIME");
            modelBuilder.Entity<Claim>().Property(x => x.DeleteDate).HasColumnName("DELETEDATE");
            modelBuilder.Entity<Claim>().Property(x => x.DeleteDate).HasColumnType("DATETIME");
            modelBuilder.Entity<Claim>().Property(x => x.ClaimTypeId).HasColumnName("CLAIMTYPEID");
            modelBuilder.Entity<Claim>().Property(x => x.Value).HasColumnName("VALUE");
            modelBuilder.Entity<Claim>().Property(x => x.Value).HasColumnType("NVARCHAR(50)");
            modelBuilder.Entity<Claim>().HasOne(x => x.ClaimType);
            modelBuilder.Entity<Claim>().HasOne(x => x.User);

            modelBuilder.Entity<ClaimType>().ToTable("CLAIMTYPES");
            modelBuilder.Entity<ClaimType>().HasKey(x => x.Id);
            modelBuilder.Entity<ClaimType>().Property(x => x.Id).HasColumnName("ID");
            modelBuilder.Entity<ClaimType>().Property(x => x.Name).HasColumnName("NAME");
            modelBuilder.Entity<ClaimType>().Property(x => x.Name).HasColumnType("NVARCHAR(500)");
            modelBuilder.Entity<ClaimType>().Property(x => x.CreateDate).HasColumnName("CREATEDATE");
            modelBuilder.Entity<ClaimType>().Property(x => x.CreateDate).HasColumnType("DATETIME");
            modelBuilder.Entity<ClaimType>().Property(x => x.UpdateDate).HasColumnName("UPDATEDATE");
            modelBuilder.Entity<ClaimType>().Property(x => x.UpdateDate).HasColumnType("DATETIME");
            modelBuilder.Entity<ClaimType>().Property(x => x.DeleteDate).HasColumnName("DELETEDATE");
            modelBuilder.Entity<ClaimType>().Property(x => x.DeleteDate).HasColumnType("DATETIME");
            modelBuilder.Entity<ClaimType>().HasMany(x => x.Claims).WithOne(x => x.ClaimType).HasForeignKey(x => x.ClaimTypeId);

            modelBuilder.Entity<Policy>().ToTable("POLICIES");
            modelBuilder.Entity<Policy>().HasKey(x => x.Id);
            modelBuilder.Entity<Policy>().Property(x => x.Id).HasColumnName("ID");
            modelBuilder.Entity<Policy>().Property(x => x.Name).HasColumnName("NAME");
            modelBuilder.Entity<Policy>().Property(x => x.Name).HasColumnType("NVARCHAR(50)");
            modelBuilder.Entity<Policy>().Property(x => x.CreateDate).HasColumnName("CREATEDATE");
            modelBuilder.Entity<Policy>().Property(x => x.CreateDate).HasColumnType("DATETIME");
            modelBuilder.Entity<Policy>().Property(x => x.UpdateDate).HasColumnName("UPDATEDATE");
            modelBuilder.Entity<Policy>().Property(x => x.UpdateDate).HasColumnType("DATETIME");
            modelBuilder.Entity<Policy>().Property(x => x.DeleteDate).HasColumnName("DELETEDATE");
            modelBuilder.Entity<Policy>().Property(x => x.DeleteDate).HasColumnType("DATETIME");
            modelBuilder.Entity<Policy>().HasMany(x => x.PolicyRoles).WithOne(x => x.Policy).HasForeignKey(x => x.PolicyId);

            modelBuilder.Entity<PolicyRole>().ToTable("POLICYROLES");
            modelBuilder.Entity<PolicyRole>().HasKey(x => x.Id);
            modelBuilder.Entity<PolicyRole>().Property(x => x.Id).HasColumnName("ID");
            modelBuilder.Entity<PolicyRole>().Property(x => x.CreateDate).HasColumnName("CREATEDATE");
            modelBuilder.Entity<PolicyRole>().Property(x => x.CreateDate).HasColumnType("DATETIME");
            modelBuilder.Entity<PolicyRole>().Property(x => x.UpdateDate).HasColumnName("UPDATEDATE");
            modelBuilder.Entity<PolicyRole>().Property(x => x.UpdateDate).HasColumnType("DATETIME");
            modelBuilder.Entity<PolicyRole>().Property(x => x.DeleteDate).HasColumnName("DELETEDATE");
            modelBuilder.Entity<PolicyRole>().Property(x => x.DeleteDate).HasColumnType("DATETIME");
            modelBuilder.Entity<PolicyRole>().Property(x => x.PolicyId).HasColumnName("POLICYID");
            modelBuilder.Entity<PolicyRole>().Property(x => x.RoleId).HasColumnName("ROLEID");
            modelBuilder.Entity<PolicyRole>().HasOne(x => x.Policy);
            modelBuilder.Entity<PolicyRole>().HasOne(x => x.Role);

            modelBuilder.Entity<Role>().ToTable("ROLES");
            modelBuilder.Entity<Role>().HasKey(x => x.Id);
            modelBuilder.Entity<Role>().Property(x => x.Id).HasColumnName("ID");
            modelBuilder.Entity<Role>().Property(x => x.Name).HasColumnName("NAME");
            modelBuilder.Entity<Role>().Property(x => x.Name).HasColumnType("NVARCHAR(50)");
            modelBuilder.Entity<Role>().Property(x => x.CreateDate).HasColumnName("CREATEDATE");
            modelBuilder.Entity<Role>().Property(x => x.CreateDate).HasColumnType("DATETIME");
            modelBuilder.Entity<Role>().Property(x => x.UpdateDate).HasColumnName("UPDATEDATE");
            modelBuilder.Entity<Role>().Property(x => x.UpdateDate).HasColumnType("DATETIME");
            modelBuilder.Entity<Role>().Property(x => x.DeleteDate).HasColumnName("DELETEDATE");
            modelBuilder.Entity<Role>().Property(x => x.DeleteDate).HasColumnType("DATETIME");
            modelBuilder.Entity<Role>().HasMany(x => x.PolicyRoles).WithOne(x => x.Role).HasForeignKey(x => x.RoleId);

            modelBuilder.Entity<Session>().ToTable("SESSIONS");
            modelBuilder.Entity<Session>().HasKey(x => x.Id);
            modelBuilder.Entity<Session>().Property(x => x.Id).HasColumnName("ID");
            modelBuilder.Entity<Session>().Property(x => x.CreateDate).HasColumnName("CREATEDATE");
            modelBuilder.Entity<Session>().Property(x => x.CreateDate).HasColumnType("DATETIME");
            modelBuilder.Entity<Session>().Property(x => x.UpdateDate).HasColumnName("UPDATEDATE");
            modelBuilder.Entity<Session>().Property(x => x.UpdateDate).HasColumnType("DATETIME");
            modelBuilder.Entity<Session>().Property(x => x.DeleteDate).HasColumnName("DELETEDATE");
            modelBuilder.Entity<Session>().Property(x => x.DeleteDate).HasColumnType("DATETIME");
            modelBuilder.Entity<Session>().Property(x => x.IpAddress).HasColumnName("IPADDRESS");
            modelBuilder.Entity<Session>().Property(x => x.IsValid).HasColumnName("ISVALID");
            modelBuilder.Entity<Session>().Property(x => x.Token).HasColumnName("TOKEN");
            modelBuilder.Entity<Session>().Property(x => x.UserAgent).HasColumnName("USERAGENT");
            modelBuilder.Entity<Session>().Property(x => x.UserId).HasColumnName("USERID");
            modelBuilder.Entity<Session>().Property(x => x.Region).HasColumnName("REGION");
            modelBuilder.Entity<Session>().Property(x => x.Region).HasColumnType("NVARCHAR(50)");
            modelBuilder.Entity<Session>().Property(x => x.ValidTo).HasColumnName("VALIDTO");
            modelBuilder.Entity<Session>().Property(x => x.RefreshIndex).HasColumnName("REFRESHINDEX");
            modelBuilder.Entity<Session>().Property(x => x.RefreshToken).HasColumnName("REFRESHTOKEN");
            modelBuilder.Entity<Session>().Property(x => x.RefreshToken).HasColumnType("NVARCHAR(50)");
            modelBuilder.Entity<Session>().Property(x => x.GrantType).HasColumnName("GRANTTYPE");
            modelBuilder.Entity<Session>().Property(x => x.GrantType).HasColumnType("NVARCHAR(50)");
            modelBuilder.Entity<Session>().Property(x => x.Scope).HasColumnName("SCOPE");
            modelBuilder.Entity<Session>().Property(x => x.Scope).HasColumnType("NVARCHAR(50)");
            modelBuilder.Entity<Session>().Property(x => x.BeforeSessionId).HasColumnName("BEFORESESSIONID");
            modelBuilder.Entity<Session>().HasOne(x => x.User);

            modelBuilder.Entity<User>().ToTable("USERS");
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<User>().Property(x => x.Id).HasColumnName("ID"); 
            modelBuilder.Entity<User>().Property(x => x.CreateDate).HasColumnName("CREATEDATE");
            modelBuilder.Entity<User>().Property(x => x.CreateDate).HasColumnType("DATETIME");
            modelBuilder.Entity<User>().Property(x => x.UpdateDate).HasColumnName("UPDATEDATE");
            modelBuilder.Entity<User>().Property(x => x.UpdateDate).HasColumnType("DATETIME");
            modelBuilder.Entity<User>().Property(x => x.DeleteDate).HasColumnName("DELETEDATE");
            modelBuilder.Entity<User>().Property(x => x.DeleteDate).HasColumnType("DATETIME");
            modelBuilder.Entity<User>().Property(x => x.Email).HasColumnName("EMAIL");
            modelBuilder.Entity<User>().Property(x => x.Email).HasColumnType("NVARCHAR(250)");
            modelBuilder.Entity<User>().Property(x => x.Password).HasColumnName("PASSWORD");
            modelBuilder.Entity<User>().Property(x => x.Password).HasColumnType("NVARCHAR(250)");
            modelBuilder.Entity<User>().HasMany(x => x.Claims).WithOne(x => x.User).HasForeignKey(x => x.UserId);
            modelBuilder.Entity<User>().HasMany(x => x.Sessions).WithOne(x => x.User).HasForeignKey(x => x.UserId);
            modelBuilder.Entity<User>().HasMany(x => x.UserRoles).WithOne(x => x.User).HasForeignKey(x => x.UserId);

            modelBuilder.Entity<UserRole>().ToTable("USERROLES");
            modelBuilder.Entity<UserRole>().HasKey(x => x.Id);
            modelBuilder.Entity<UserRole>().Property(x => x.Id).HasColumnName("ID");
            modelBuilder.Entity<UserRole>().Property(x => x.CreateDate).HasColumnName("CREATEDATE");
            modelBuilder.Entity<UserRole>().Property(x => x.CreateDate).HasColumnType("DATETIME");
            modelBuilder.Entity<UserRole>().Property(x => x.UpdateDate).HasColumnName("UPDATEDATE");
            modelBuilder.Entity<UserRole>().Property(x => x.UpdateDate).HasColumnType("DATETIME");
            modelBuilder.Entity<UserRole>().Property(x => x.DeleteDate).HasColumnName("DELETEDATE");
            modelBuilder.Entity<UserRole>().Property(x => x.DeleteDate).HasColumnType("DATETIME");
            modelBuilder.Entity<UserRole>().Property(x => x.UserId).HasColumnName("USERID");
            modelBuilder.Entity<UserRole>().Property(x => x.RoleId).HasColumnName("ROLEID");
            modelBuilder.Entity<UserRole>().HasOne(x => x.User);
            modelBuilder.Entity<UserRole>().HasOne(x => x.Role);

            base.OnModelCreating(modelBuilder);
        }
    }
}
