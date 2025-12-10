using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Data.EFCore
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Status)
                      .HasConversion<string>();

                entity.HasOne<Category>()           
                       .WithMany()                     
                       .HasForeignKey(p => p.CategoryId) 
                       .IsRequired()                   
                       .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
