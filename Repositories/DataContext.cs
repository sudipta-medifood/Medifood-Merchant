using Microsoft.EntityFrameworkCore;
using Models.Merchants;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using Models.Interface;

namespace Repositories
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<MerchantRestaurant> MerchantRestaurants { get; set; }
        public DbSet<MerchantPharmacy> MerchantPharmacys { get; set; }
        public DbSet<RefTokenPharmacyMerchant> RefTokenPharmacyMerchants { get; set; }
        public DbSet<RefTokenRestaurantMerchant> RefTokenRestaurantMerchants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //configure RefreshToken entity model to implement table per hierarchy(TPH)
            modelBuilder.Entity<RefreshToken>().ToTable("RefreshTokens");
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSavingData();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        private void OnBeforeSavingData()
        {
            var entries = ChangeTracker.Entries().Where(e => e.State != EntityState.Detached && e.State != EntityState.Unchanged);

            foreach (var entry in entries)
            {
                if (entry.Entity is ITrackable trackable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            trackable.CreatedAt = DateTimeOffset.Now;
                            trackable.LastUpdatedAt = DateTimeOffset.Now;
                            break;
                        case EntityState.Modified:
                            trackable.LastUpdatedAt = DateTimeOffset.Now;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSavingData();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
