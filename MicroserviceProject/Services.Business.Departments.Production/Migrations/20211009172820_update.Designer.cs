﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Services.Business.Departments.Production.Configuration.Persistence;

namespace Services.Business.Departments.Production.Migrations
{
    [DbContext(typeof(ProductionContext))]
    [Migration("20211009172820_update")]
    partial class update
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Services.Business.Departments.Production.Entities.EntityFramework.ProductDependencyEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Amount")
                        .HasColumnType("int")
                        .HasColumnName("AMOUNT");

                    b.Property<DateTime?>("DeleteDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DependedProductId")
                        .HasColumnType("int")
                        .HasColumnName("DEPENDED_PRODUCT_ID");

                    b.Property<int>("ProductId")
                        .HasColumnType("int")
                        .HasColumnName("PRODUCT_ID");

                    b.HasKey("Id");

                    b.HasIndex("DependedProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("PRODUCTION_PRODUCT_DEPENDENCIES");
                });

            modelBuilder.Entity("Services.Business.Departments.Production.Entities.EntityFramework.ProductEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("DeleteDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProductId")
                        .HasColumnType("int")
                        .HasColumnName("PRODUCT_ID");

                    b.Property<string>("ProductName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("PRODUCT_NAME");

                    b.HasKey("Id");

                    b.ToTable("PRODUCTION_PRODUCTS");
                });

            modelBuilder.Entity("Services.Business.Departments.Production.Entities.EntityFramework.RollbackEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("DeleteDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRolledback")
                        .HasColumnType("bit")
                        .HasColumnName("ISROLLEDBACK");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("DATETIME")
                        .HasColumnName("TRANSACTION_DATE");

                    b.Property<string>("TransactionIdentity")
                        .HasColumnType("NVARCHAR(100)")
                        .HasColumnName("TRANSACTION_IDENTITY");

                    b.Property<int>("TransactionType")
                        .HasColumnType("int")
                        .HasColumnName("TRANSACTION_TYPE");

                    b.HasKey("Id");

                    b.ToTable("PRODUCTION_TRANSACTIONS");
                });

            modelBuilder.Entity("Services.Business.Departments.Production.Entities.EntityFramework.RollbackItemEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DataSet")
                        .HasColumnType("NVARCHAR(50)")
                        .HasColumnName("DATA_SET");

                    b.Property<DateTime?>("DeleteDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Identity")
                        .HasColumnType("NVARCHAR(50)")
                        .HasColumnName("IDENTITY_");

                    b.Property<bool>("IsRolledback")
                        .HasColumnType("bit")
                        .HasColumnName("ISROLLEDBACK");

                    b.Property<string>("Name")
                        .HasColumnType("NVARCHAR(250)")
                        .HasColumnName("NAME");

                    b.Property<string>("NewValue")
                        .HasColumnType("NVARCHAR(250)")
                        .HasColumnName("NEW_VALUE");

                    b.Property<string>("OldValue")
                        .HasColumnType("NVARCHAR(250)")
                        .HasColumnName("OLD_VALUE");

                    b.Property<int?>("RollbackEntityId")
                        .HasColumnType("int");

                    b.Property<string>("RollbackType")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(250)")
                        .HasColumnName("ROLLBACK_TYPE");

                    b.Property<string>("TransactionIdentity")
                        .HasColumnType("NVARCHAR(100)")
                        .HasColumnName("TRANSACTION_IDENTITY");

                    b.HasKey("Id");

                    b.HasIndex("RollbackEntityId");

                    b.ToTable("PRODUCTION_TRANSACTION_ITEMS");
                });

            modelBuilder.Entity("Services.Business.Departments.Production.Entities.EntityFramework.ProductDependencyEntity", b =>
                {
                    b.HasOne("Services.Business.Departments.Production.Entities.EntityFramework.ProductEntity", "DependedProduct")
                        .WithMany("ProductDependenciesForDependency")
                        .HasForeignKey("DependedProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Services.Business.Departments.Production.Entities.EntityFramework.ProductEntity", "Product")
                        .WithMany("ProductDependenciesForProduct")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DependedProduct");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Services.Business.Departments.Production.Entities.EntityFramework.RollbackItemEntity", b =>
                {
                    b.HasOne("Services.Business.Departments.Production.Entities.EntityFramework.RollbackEntity", null)
                        .WithMany("RollbackItems")
                        .HasForeignKey("RollbackEntityId");
                });

            modelBuilder.Entity("Services.Business.Departments.Production.Entities.EntityFramework.ProductEntity", b =>
                {
                    b.Navigation("ProductDependenciesForDependency");

                    b.Navigation("ProductDependenciesForProduct");
                });

            modelBuilder.Entity("Services.Business.Departments.Production.Entities.EntityFramework.RollbackEntity", b =>
                {
                    b.Navigation("RollbackItems");
                });
#pragma warning restore 612, 618
        }
    }
}
