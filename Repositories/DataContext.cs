﻿using Microsoft.EntityFrameworkCore;
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
        public DbSet<PharmacyMerchantProfile> PharmacyMerchantProfiles { get; set; }
        public DbSet<RestaurantMerchantProfile> RestaurantMerchantProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //configure RefreshToken entity model to implement table per hierarchy(TPH)
            modelBuilder.Entity<RefreshToken>().ToTable("RefreshTokens");

            //one to one relationship
            modelBuilder.Entity<MerchantPharmacy>()
                .HasOne(a => a.PharmacyMerchantProfiles)
                .WithOne(b => b.MerchantPharmacy)
                .HasForeignKey<PharmacyMerchantProfile>(c => c.PharmacyMerchantId);
            modelBuilder.Entity<MerchantRestaurant>()
                .HasOne(a => a.RestaurantMerchantProfiles)
                .WithOne(b => b.MerchantRestaurant)
                .HasForeignKey<RestaurantMerchantProfile>(c => c.RestaurantMerchantId);
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
                            trackable.CreatedAt = DateTime.UtcNow;
                            trackable.LastUpdatedAt = DateTime.UtcNow;
                            break;
                        case EntityState.Modified:
                            trackable.LastUpdatedAt = DateTime.UtcNow;
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
