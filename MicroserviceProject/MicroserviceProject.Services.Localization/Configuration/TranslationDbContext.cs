using MicroserviceProject.Services.Localization.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using System;
using System.Linq;

namespace MicroserviceProject.Infrastructure.Localization.Persistence.Configuration
{
    /// <summary>
    /// Dil çevirilerine ait veritabanı bağlantısı context sınıfı
    /// </summary>
    public class TranslationDbContext : DbContext
    {
        /// <summary>
        /// Yapılandırma bilgilerinin alınacağı configuration nesnesi
        /// </summary>
        private readonly IConfiguration configuration;

        public virtual DbSet<TranslationEntity> Translations { get; set; }

        public TranslationDbContext() : base()
        {

        }

        public TranslationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }

        /// <summary>
        /// Dil çevirilerine ait veritabanı bağlantısı context sınıfı
        /// </summary>
        /// <param name="configuration">Yapılandırma bilgilerinin alınacağı configuration nesnesi</param>
        public TranslationDbContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("server=localhost;DataBase=MicroserviceDB;user=sa;password=Srkn_CMR*1987;MultipleActiveResultSets=true");

            optionsBuilder.UseSqlServer(
                configuration
                .GetSection("Configuration")
                .GetSection("Localization")
                .GetSection("TranslationDbConnnectionString")
                .Value);

            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TranslationEntity>(x =>
            {
                x.HasIndex(y => new { y.Key, y.LanguageCode }).IsUnique();
            });

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry in ChangeTracker.Entries().Where(x => x.Entity is BaseEntity).ToList())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
                {
                    if (entry.State == EntityState.Added)
                    {
                        (entry.Entity as BaseEntity).CreateDate = DateTime.Now;
                    }
                    if (entry.State == EntityState.Deleted)
                    {
                        entry.State = EntityState.Modified;
                        (entry.Entity as BaseEntity).DeleteDate = DateTime.Now;
                    }
                    (entry.Entity as BaseEntity).UpdateDate = DateTime.Now;

                }
            }
            return base.SaveChanges();
        }
    }
}
