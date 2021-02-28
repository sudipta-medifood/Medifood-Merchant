using Microsoft.EntityFrameworkCore;
using Models.Riders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Models.Interface;

namespace Repositories
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Rider> Riders { get; set; }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSavingData();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        private void OnBeforeSavingData()
        {
            var entries = ChangeTracker.Entries().Where(e => e.State != EntityState.Detached && e.State != EntityState.Unchanged);

            foreach(var entry in entries)
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
