﻿// <auto-generated />
using System;
using FDownl_Shared_Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FDownl_Shared_Resources.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20210721165807_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.8");

            modelBuilder.Entity("FDownl_Shared_Resources.Models.StorageServer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Hostname")
                        .HasColumnType("longtext");

                    b.Property<string>("Ip")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsFull")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Location")
                        .HasColumnType("longtext");

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

                    b.Property<int>("Lifetime")
                        .HasColumnType("int");

                    b.Property<string>("RandomId")
                        .HasColumnType("longtext");

                    b.Property<string>("ServerName")
                        .HasColumnType("longtext");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UploadedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("UploadedFiles");
                });
#pragma warning restore 612, 618
        }
    }
}
