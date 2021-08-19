﻿// <auto-generated />
using System;
using FDownl_Shared_Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FDownl_Shared_Resources.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.8");

            modelBuilder.Entity("FDownl_Shared_Resources.Models.CouponCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .HasColumnType("longtext");

                    b.Property<int>("LifetimeAdd")
                        .HasColumnType("int");

                    b.Property<int>("LifetimeSet")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("CouponCodes");
                });

            modelBuilder.Entity("FDownl_Shared_Resources.Models.StorageServer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Hostname")
                        .HasColumnType("longtext");

                    b.Property<string>("Ip")
                        .HasColumnType("longtext");

                    b.Property<string>("Location")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp(6)");

                    b.HasKey("Id");

                    b.ToTable("StorageServers");
                });

            modelBuilder.Entity("FDownl_Shared_Resources.Models.UploadedFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Coupon")
                        .HasColumnType("longtext");

                    b.Property<string>("Filename")
                        .HasColumnType("longtext");

                    b.Property<string>("Hostname")
                        .HasColumnType("longtext");

                    b.Property<string>("Ip")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsEncrypted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Lifetime")
                        .HasColumnType("int");

                    b.Property<string>("RandomId")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp(6)");

                    b.Property<string>("ServerName")
                        .HasColumnType("longtext");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UploadedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ZipContents")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("UploadedFiles");
                });
#pragma warning restore 612, 618
        }
    }
}
