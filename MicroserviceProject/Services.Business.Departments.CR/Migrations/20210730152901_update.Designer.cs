﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Services.Business.Departments.CR.Configuration.Persistence;

namespace Services.Business.Departments.CR.Migrations
{
    [DbContext(typeof(CRContext))]
    [Migration("20210730152901_update")]
    partial class update
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Services.Business.Departments.CR.Entities.EntityFramework.CustomerEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("DeleteDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsLegal")
                        .HasColumnType("bit")
                        .HasColumnName("ISLEGAL");

                    b.Property<string>("MiddleName")
                        .HasColumnType("NVARCHAR(50)")
                        .HasColumnName("MIDDLENAME");

                    b.Property<string>("Name")
                        .HasColumnType("NVARCHAR(50)")
                        .HasColumnName("NAME");

                    b.Property<string>("Surname")
                        .HasColumnType("NVARCHAR(100)")
                        .HasColumnName("SURNAME");

                    b.HasKey("Id");

                    b.ToTable("CR_CUSTOMERS");
                });
#pragma warning restore 612, 618
        }
    }
}
