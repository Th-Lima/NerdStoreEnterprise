﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NSE.Payment.API.Data;

namespace NSE.Payment.API.Migrations
{
    [DbContext(typeof(PaymentContext))]
    [Migration("20230504022552_Payments")]
    partial class Payments
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NSE.Payment.API.Models.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("TotalValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("TypePayment")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("NSE.Payment.API.Models.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AuthorizationCode")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("CardBrand")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("NSU")
                        .HasColumnType("varchar(100)");

                    b.Property<Guid>("PaymentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("TID")
                        .HasColumnType("varchar(100)");

                    b.Property<decimal>("TotalValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TransactionCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PaymentId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("NSE.Payment.API.Models.Transaction", b =>
                {
                    b.HasOne("NSE.Payment.API.Models.Payment", "Payment")
                        .WithMany("Transactions")
                        .HasForeignKey("PaymentId")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
