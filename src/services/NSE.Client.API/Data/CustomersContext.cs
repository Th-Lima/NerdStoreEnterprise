using Microsoft.EntityFrameworkCore;
using NSE.Client.API.Models;
using NSE.Core.Data;
using System.Linq;
using System.Threading.Tasks;


namespace NSE.Client.API.Data
{
    public sealed class CustomersContext : DbContext, IUnitOfWork
    {
        public CustomersContext(DbContextOptions<CustomersContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addressess { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomersContext).Assembly);
        }

        public async Task<bool> Commit()
        {
            var success = await base.SaveChangesAsync() > 0;

            return success;
        }
    }
}
