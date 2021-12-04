﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Services.Api.Localization.Configuration.Persistence;

namespace Services.Api.Localization.Migrations
{
    [DbContext(typeof(TranslationDbContext))]
    [Migration("20211204174840_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Services.Api.Localization.Entities.TranslationEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INT")
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("DATETIME")
                        .HasColumnName("CREATEDATE");

                    b.Property<DateTime?>("DeleteDate")
                        .HasColumnType("DATETIME")
                        .HasColumnName("DELETEDATE");

                    b.Property<string>("Key")
                        .HasMaxLength(250)
                        .HasColumnType("NVARCHAR(250)")
                        .HasColumnName("KEY");

                    b.Property<string>("Region")
                        .HasMaxLength(50)
                        .HasColumnType("NVARCHAR(50)")
                        .HasColumnName("REGION");

                    b.Property<string>("Text")
                        .HasMaxLength(4000)
                        .HasColumnType("NVARCHAR(4000)")
                        .HasColumnName("TEXT");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("DATETIME")
                        .HasColumnName("UPDATEDATE");

                    b.HasKey("Id");

                    b.HasIndex("Key", "Region")
                        .IsUnique()
                        .HasFilter("[KEY] IS NOT NULL AND [REGION] IS NOT NULL");

                    b.ToTable("TRANSLATIONS");
                });
#pragma warning restore 612, 618
        }
    }
}
