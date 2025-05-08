using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ETicaretAPI.Persistence.Contexts
{
    public class ETicaretDbContext : DbContext
    {
        public ETicaretDbContext(DbContextOptions options) : base(options)
        {}
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }

        //interceptors
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeTracker.Entries<BaseEntity>().ToList()
                .ForEach(e =>
                {
                    if (e.State == EntityState.Added)
                    {
                        e.Entity.CreatedDate = DateTime.UtcNow;
                    }
                    else if (e.State == EntityState.Modified)
                    {
                        e.Entity.UpdatedDate = DateTime.UtcNow;
                    }
                });
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}