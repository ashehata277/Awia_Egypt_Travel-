using AwiaEgyptTravel.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace AwiaEgyptTravel.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Tour> Tours { get; set; }
        public DbSet<TourPhoto> TourPhotos { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tour>()
                .HasMany(t => t.Photos)
                .WithOne(p => p.Tour)
                .HasForeignKey(p => p.TourId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Tour>()
                .HasMany(t => t.Orders)
                .WithOne(o => o.Tour)
                .HasForeignKey(o => o.TourId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmailTemplate>()
                .HasIndex(e => e.IsDefault);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
