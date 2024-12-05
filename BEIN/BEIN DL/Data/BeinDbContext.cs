using BEIN_DL.Models;
using Microsoft.EntityFrameworkCore;

namespace BEIN_DL.Data
{
    public class BeinDbContext(DbContextOptions<BeinDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<SoftwareProduct> SoftwareProducts { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Visit> Visits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<SoftwareProduct>().ToTable("Softwares");
            modelBuilder.Entity<Visit>().ToTable("Visits");
            modelBuilder.Entity<Feature>().ToTable("Features");

            modelBuilder.Entity<User>()
                .HasMany(visits => visits.Visits)
                .WithOne(user => user.User)
                .HasForeignKey(visits => visits.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Visit>()
                .HasOne(user => user.User)
                .WithMany(visits => visits.Visits)
                .HasForeignKey(visits => visits.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SoftwareProduct>()
                .HasMany(features => features.Features)
                .WithOne(product => product.SoftwareProduct)
                .HasForeignKey(features => features.SoftwareProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SoftwareProduct>()
                .HasMany(visits => visits.Visits)
                .WithOne(product => product.Product)
                .HasForeignKey(visit => visit.SoftwareProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Visit>()
                .HasOne(product => product.Product)
                .WithMany(visits => visits.Visits)
                .HasForeignKey(visit => visit.SoftwareProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Visit>()
                .HasKey(v => new { v.UserId, v.SoftwareProductId });
        }
    }
}
