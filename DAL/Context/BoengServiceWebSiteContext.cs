using System;
using System.Configuration;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DAL.Context
{
    public partial class BoengServiceWebSiteContext : DbContext
    {
        public BoengServiceWebSiteContext()
        {
            Database.Migrate();
        }

        public BoengServiceWebSiteContext(DbContextOptions<BoengServiceWebSiteContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PlaneModel> PlaneModels { get; set; }
        public virtual DbSet<PlanePart> PlaneParts { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(true);
            if (!optionsBuilder.IsConfigured)
            {
#if DEBUG
                string applicationName =
                    Environment.GetCommandLineArgs()[0];
#else
                string applicationName =
                    SEnvironment.GetCommandLineArgs()[0]+ ".exe";
#endif

                string exePath = System.IO.Path.Combine(
                    Environment.CurrentDirectory, applicationName);

                Configuration configuration = ConfigurationManager.OpenExeConfiguration(exePath);
                optionsBuilder.UseSqlServer(configuration.AppSettings.Settings["connection"].Value);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");


            modelBuilder.Entity<PlaneModel>(entity =>
            {
                entity.ToTable("PlaneModel");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(80);
                entity.HasMany(e => e.Users).WithMany(e => e.PlaneModels);
                entity.Property(e => e.YearOfProd).HasColumnType("datetime");
                entity.Property(e => e.Image);
            });

            modelBuilder.Entity<PlanePart>(entity =>
            {
                entity.ToTable("PlanePart");

                entity.Property(e => e.Manufacturer).HasMaxLength(80);

                entity.Property(e => e.ManufacturingDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(80);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.PlaneModel)
                    .WithMany(p => p.PlaneParts)
                    .HasForeignKey(d => d.PlaneModelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlanePart_PlaneModel");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.Property(e => e.Password).IsRequired().HasMaxLength(32).HasColumnType("binary");
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(40);
                entity.HasMany(e => e.PlaneModels).WithMany(e => e.Users);
                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");
            });

        }

    }
}
