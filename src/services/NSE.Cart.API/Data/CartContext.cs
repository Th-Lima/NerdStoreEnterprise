﻿using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NSE.Cart.API.Models;
using System.Linq;

namespace NSE.Cart.API.Data
{
    public class CartContext : DbContext
    {
        public CartContext(DbContextOptions<CartContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<CartCustomer> CartCustomers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var modelBuilderProperties = modelBuilder.Model.GetEntityTypes().SelectMany(x => x.GetProperties().Where(x => x.ClrType == typeof(string)));

            foreach (var property in modelBuilderProperties)
            {
                property.SetColumnType("varchar(100)");
            }

            modelBuilder.Ignore<ValidationResult>();

            modelBuilder.Entity<CartCustomer>()
                .HasIndex(c => c.CustomerId)
                .HasName("IDX_Customer");

            modelBuilder.Entity<CartCustomer>()
               .Ignore(c => c.Voucher)
               .OwnsOne(c => c.Voucher, v =>
               {
                   v.Property(vc => vc.Code)
                       .HasColumnName("VoucherCode")
                       .HasColumnType("varchar(50)");

                   v.Property(vc => vc.TypeDiscount)
                       .HasColumnName("TypeDiscount");

                   v.Property(vc => vc.Percentage)
                       .HasColumnName("Percentage");

                   v.Property(vc => vc.ValueDiscount)
                       .HasColumnName("ValueDiscount");
               });

            modelBuilder.Entity<CartCustomer>()
                .HasMany(c => c.Itens)
                .WithOne(i => i.CartCustomer)
                .HasForeignKey(c => c.CartId);

            foreach (var relantionship in modelBuilder.Model.GetEntityTypes().SelectMany(x => x.GetForeignKeys())) 
            {
                relantionship.DeleteBehavior = DeleteBehavior.Cascade;
            }
        }
    }
}
