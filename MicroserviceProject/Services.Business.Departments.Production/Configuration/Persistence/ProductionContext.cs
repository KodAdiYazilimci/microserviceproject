using Microsoft.EntityFrameworkCore;

using Services.Business.Departments.Production.Entities.EntityFramework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Business.Departments.Production.Configuration.Persistence
{
    public class ProductionContext : DbContext
    {
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<RollbackEntity> Rollbacks { get; set; }
        public DbSet<RollbackItemEntity> RollbackItems { get; set; }

        public ProductionContext() : base()
        {
        }

        public ProductionContext(DbContextOptions<ProductionContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server=localhost;DataBase=Microservice_Production_DB;user=sa;password=Srkn_CMR*1987;MultipleActiveResultSets=true");

                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.EnableDetailedErrors();
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductEntity>().ToTable("PRODUCTION_PRODUCTS");
            modelBuilder.Entity<ProductEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<ProductEntity>().Property(x => x.ProductId).HasColumnName("PRODUCT_ID");
            modelBuilder.Entity<ProductEntity>().Property(x => x.ProductName).HasColumnName("PRODUCT_NAME");

            modelBuilder.Entity<RollbackEntity>().ToTable("PRODUCTION_TRANSACTIONS");

            modelBuilder.Entity<RollbackEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<RollbackEntity>().Property(x => x.Id).HasColumnName("ID");
            modelBuilder.Entity<RollbackEntity>().Property(x => x.IsRolledback).HasColumnName("ISROLLEDBACK");
            modelBuilder.Entity<RollbackEntity>().Property(x => x.TransactionDate).HasColumnName("TRANSACTION_DATE");
            modelBuilder.Entity<RollbackEntity>().Property(x => x.TransactionDate).HasColumnType("DATETIME");
            modelBuilder.Entity<RollbackEntity>().Property(x => x.TransactionIdentity).HasColumnName("TRANSACTION_IDENTITY");
            modelBuilder.Entity<RollbackEntity>().Property(x => x.TransactionIdentity).HasColumnType("NVARCHAR(100)");
            modelBuilder.Entity<RollbackEntity>().Property(x => x.TransactionType).HasColumnName("TRANSACTION_TYPE");

            modelBuilder.Entity<RollbackItemEntity>().ToTable("PRODUCTION_TRANSACTION_ITEMS");

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
