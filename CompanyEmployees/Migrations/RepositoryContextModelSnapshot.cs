﻿// <auto-generated />
using System;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CompanyEmployees.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    partial class RepositoryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Entities.Models.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CompanyId");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("Id");

                    b.ToTable("Compamies");

                    b.HasData(
                        new
                        {
                            Id = new Guid("ed62a714-b958-4e64-a399-c4cb975dcd58"),
                            Address = "She9 andre9",
                            Country = "Liberty",
                            Name = "Dvoinoi podborodok"
                        },
                        new
                        {
                            Id = new Guid("7b2e2490-0148-41a3-ba6a-6da08c699d7b"),
                            Address = "U kajdogo vtorogo",
                            Country = "Meste4kova9",
                            Name = "ShershaVa9 p9tka"
                        });
                });

            modelBuilder.Entity("Entities.Models.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("EmployeeId");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            Id = new Guid("e375886b-07ce-4771-86b5-3dc3a62b3e23"),
                            Age = 47,
                            CompanyId = new Guid("ed62a714-b958-4e64-a399-c4cb975dcd58"),
                            Name = "Jack Jons",
                            Position = "Manager"
                        },
                        new
                        {
                            Id = new Guid("b69883db-64ee-467b-8e07-1ffd6c1545be"),
                            Age = 31,
                            CompanyId = new Guid("ed62a714-b958-4e64-a399-c4cb975dcd58"),
                            Name = "Dora Nemo",
                            Position = "HR"
                        },
                        new
                        {
                            Id = new Guid("1196e3e2-feff-40d1-b88b-4074f3c90d1f"),
                            Age = 18,
                            CompanyId = new Guid("ed62a714-b958-4e64-a399-c4cb975dcd58"),
                            Name = "Skoobi Doo",
                            Position = "Barista"
                        },
                        new
                        {
                            Id = new Guid("a4ac1859-e888-4bc7-a94b-40814190b679"),
                            Age = 59,
                            CompanyId = new Guid("7b2e2490-0148-41a3-ba6a-6da08c699d7b"),
                            Name = "Bazon Higgs",
                            Position = "World Developer"
                        });
                });

            modelBuilder.Entity("Entities.Models.Employee", b =>
                {
                    b.HasOne("Entities.Models.Company", "Company")
                        .WithMany("Employees")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Entities.Models.Company", b =>
                {
                    b.Navigation("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}
