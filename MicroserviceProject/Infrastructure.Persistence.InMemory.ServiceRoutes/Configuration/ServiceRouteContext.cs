using Infrastructure.Persistence.InMemory.ServiceRoutes.Persistence;

using MicroserviceProject.Model.Communication.Moderator;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.InMemory.ServiceRoutes.Configuration
{
    /// <summary>
    /// Micro servis endpoint bilgilerini tutan inmemory veritabanı
    /// </summary>
    public class ServiceRouteContext : DbContext
    {
        /// <summary>
        /// Servis çağrı modelleri
        /// </summary>
        public DbSet<CallModel> CallModels { get; set; }

        /// <summary>
        /// Servis çağrılarına ait query string anahtarları
        /// </summary>
        public DbSet<QueryKey> QueryKeys { get; set; }

        /// <summary>
        /// Micro servis endpoint bilgilerini tutan inmemory veritabanı
        /// </summary>
        public ServiceRouteContext() : base()
        {
            Seed seed = new Seed();

            seed.InsertInitialData(this);
        }

        /// <summary>
        /// Micro servis endpoint bilgilerini tutan inmemory veritabanı
        /// </summary>
        /// <param name="dbContextOptions">Veritabanı yapılandırması</param>
        public ServiceRouteContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            Seed seed = new Seed();

            seed.InsertInitialData(this);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseInMemoryDatabase("ServiceRoutesInMemoryDB");

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
