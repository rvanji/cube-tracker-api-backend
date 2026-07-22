namespace CubeTrackerAPI.Data
{
    using CubeTrackerAPI.Models;
    using Microsoft.EntityFrameworkCore;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<DailyReturn> DailyReturns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DailyReturn>()
                .Property(d => d.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<DailyReturn>()
                .Property(d => d.CubeRate)
                .HasColumnType("decimal(18,2)");
        }
    }
}
