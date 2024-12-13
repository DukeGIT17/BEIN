using BEIN_DL.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace BEIN_DL.Data
{
    public class BeinDbContext(DbContextOptions<BeinDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<SoftwareProduct> Softwares { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<Sector> Sectors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SoftwareProduct>().ToTable("Softwares");

            modelBuilder.Entity<User>()
                .HasMany(visits => visits.Visits)
                .WithOne(user => user.User)
                .HasForeignKey(visits => visits.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique();
            
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Visit>()
                .HasOne(user => user.User)
                .WithMany(visits => visits.Visits)
                .HasForeignKey(visits => visits.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SoftwareProduct>()
                .HasKey(sp => sp.Id);

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

            modelBuilder.Entity<SoftwareProduct>()
                .HasMany(sectors => sectors.Sectors)
                .WithOne(product => product.Product)
                .HasForeignKey(sector => sector.SectorId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            modelBuilder.Entity<Visit>()
                .HasOne(product => product.Product)
                .WithMany(visits => visits.Visits)
                .HasForeignKey(visit => visit.SoftwareProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Visit>()
                .HasKey(v => new { v.UserId, v.SoftwareProductId });

            modelBuilder.Entity<Sector>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<Sector>()
                .HasMany(products => products.Products)
                .WithOne(sector => sector.Sector)
                .HasForeignKey(sector => sector.ProductId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            modelBuilder.Entity<Sector>()
                .HasOne(sectorInfo => sectorInfo.SectorInformation)
                .WithOne(sector => sector.Sector)
                .HasForeignKey<SectorInformation>(s => s.SectorId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<Sector>()
                .HasIndex(s => s.Title)
                .IsUnique();

            modelBuilder.Entity<SectorProduct>()
                .HasKey(sp => new { sp.ProductId, sp.SectorId });

            modelBuilder.Entity<SectorInformation>()
                .HasKey(si => si.Id);

            modelBuilder.Entity<SectorInformation>()
                .HasMany(ci => ci.CardInformation)
                .WithOne(si => si.SectorInformation)
                .HasForeignKey(ci => ci.SectorInformationId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<SectorInformation>()
                .HasMany(sp => sp.SectorPrinciples)
                .WithOne(si => si.SectorInformation)
                .HasForeignKey(sp => sp.SectorInformationId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<CardInfo>()
                .HasKey(ci => ci.Id);

            modelBuilder.Entity<Feature>()
                .HasKey(f => f.Id);

            modelBuilder.Entity<SectorPrinciple>()
                .HasKey(sp => sp.Id);
        }
    }
}
