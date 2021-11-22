using Microsoft.EntityFrameworkCore;

using Services.Api.Business.Departments.CR.Entities.EntityFramework;

namespace Services.Api.Business.Departments.CR.Configuration.Persistence
{
    public class CRContext : DbContext
    {
        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<RollbackEntity> Rollbacks { get; set; }
        public DbSet<RollbackItemEntity> RollbackItems { get; set; }

        public CRContext() : base()
        {
        }

        public CRContext(DbContextOptions<CRContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server=localhost;DataBase=Microservice_CR_DB;user=sa;password=Srkn_CMR*1987;MultipleActiveResultSets=true");

                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.EnableDetailedErrors();
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerEntity>().ToTable("CR_CUSTOMERS");

            modelBuilder.Entity<CustomerEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<CustomerEntity>().Property(x => x.Id).HasColumnName("ID");

            modelBuilder.Entity<CustomerEntity>().Ignore(x => x.TypeOfPerson);

            modelBuilder.Entity<CustomerEntity>().Property(x => x.IsLegal).HasColumnName("ISLEGAL");
            modelBuilder.Entity<CustomerEntity>().Property(x => x.Name).HasColumnName("NAME");
            modelBuilder.Entity<CustomerEntity>().Property(x => x.Name).HasColumnType("NVARCHAR(50)");
            modelBuilder.Entity<CustomerEntity>().Property(x => x.MiddleName).HasColumnName("MIDDLENAME");
            modelBuilder.Entity<CustomerEntity>().Property(x => x.MiddleName).HasColumnType("NVARCHAR(50)");
            modelBuilder.Entity<CustomerEntity>().Property(x => x.Surname).HasColumnName("SURNAME");
            modelBuilder.Entity<CustomerEntity>().Property(x => x.Surname).HasColumnType("NVARCHAR(100)");

            modelBuilder.Entity<RollbackEntity>().ToTable("CR_TRANSACTIONS");

            modelBuilder.Entity<RollbackEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<RollbackEntity>().Property(x => x.Id).HasColumnName("ID");
            modelBuilder.Entity<RollbackEntity>().Property(x => x.IsRolledback).HasColumnName("ISROLLEDBACK");
            modelBuilder.Entity<RollbackEntity>().Property(x => x.TransactionDate).HasColumnName("TRANSACTION_DATE");
            modelBuilder.Entity<RollbackEntity>().Property(x => x.TransactionDate).HasColumnType("DATETIME");
            modelBuilder.Entity<RollbackEntity>().Property(x => x.TransactionIdentity).HasColumnName("TRANSACTION_IDENTITY");
            modelBuilder.Entity<RollbackEntity>().Property(x => x.TransactionIdentity).HasColumnType("NVARCHAR(100)");
            modelBuilder.Entity<RollbackEntity>().Property(x => x.TransactionType).HasColumnName("TRANSACTION_TYPE");

            modelBuilder.Entity<RollbackItemEntity>().ToTable("CR_TRANSACTION_ITEMS");

            modelBuilder.Entity<RollbackItemEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<RollbackItemEntity>().Property(x => x.Id).HasColumnName("ID");
            modelBuilder.Entity<RollbackItemEntity>().Property(x => x.IsRolledback).HasColumnName("ISROLLEDBACK");
            modelBuilder.Entity<RollbackItemEntity>().Property(x => x.TransactionIdentity).HasColumnName("TRANSACTION_IDENTITY");
            modelBuilder.Entity<RollbackItemEntity>().Property(x => x.TransactionIdentity).HasColumnType("NVARCHAR(100)");
            modelBuilder.Entity<RollbackItemEntity>().Property(x => x.DataSet).HasColumnName("DATA_SET");
            modelBuilder.Entity<RollbackItemEntity>().Property(x => x.DataSet).HasColumnType("NVARCHAR(50)");
            modelBuilder.Entity<RollbackItemEntity>().Property(x => x.Identity).HasColumnName("IDENTITY_");
            modelBuilder.Entity<RollbackItemEntity>().Property(x => x.Identity).HasColumnType("NVARCHAR(50)");
            modelBuilder.Entity<RollbackItemEntity>().Property(x => x.Name).HasColumnName("NAME");
            modelBuilder.Entity<RollbackItemEntity>().Property(x => x.Name).HasColumnType("NVARCHAR(250)");
            modelBuilder.Entity<RollbackItemEntity>().Property(x => x.NewValue).HasColumnName("NEW_VALUE");
            modelBuilder.Entity<RollbackItemEntity>().Property(x => x.NewValue).HasColumnType("NVARCHAR(250)");
            modelBuilder.Entity<RollbackItemEntity>().Property(x => x.OldValue).HasColumnName("OLD_VALUE");
            modelBuilder.Entity<RollbackItemEntity>().Property(x => x.OldValue).HasColumnType("NVARCHAR(250)");
            modelBuilder.Entity<RollbackItemEntity>().Property(x => x.RollbackType).HasColumnName("ROLLBACK_TYPE");
            modelBuilder.Entity<RollbackItemEntity>().Property(x => x.RollbackType).HasColumnType("NVARCHAR(250)");

            base.OnModelCreating(modelBuilder);
        }
    }
}
