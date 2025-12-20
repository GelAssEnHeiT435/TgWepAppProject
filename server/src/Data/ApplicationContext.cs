using FlowerBot.src.Data.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace FlowerBot.src.Data
{
    public class ApplicationContext: DbContext
    {
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Image> Images => Set<Image>();

        public ApplicationContext(DbContextOptions<ApplicationContext> options): base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Image)
                .WithOne(i => i.Product)
                .HasForeignKey<Image>(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
