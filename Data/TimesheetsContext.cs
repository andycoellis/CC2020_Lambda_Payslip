using System;
using CC2020_Lambda_Payslip.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CC2020_Lambda_Payslip.Data
{
    public partial class TimesheetsContext : DbContext
    {
        public TimesheetsContext()
        {
        }

        public TimesheetsContext(DbContextOptions<TimesheetsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Companies> Companies { get; set; }
        public virtual DbSet<PayAgreements> PayAgreements { get; set; }
        public virtual DbSet<Payslips> Payslips { get; set; }
        public virtual DbSet<Timesheets> Timesheets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=main.cyzqecdhmroe.us-east-1.rds.amazonaws.com,1433;Database=Timesheets;uid=admin;pwd=CCaws2020!;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(40);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.Name).HasMaxLength(40);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.State).HasMaxLength(3);

                entity.Property(e => e.Tfn)
                    .HasColumnName("TFN")
                    .HasMaxLength(9);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<Companies>(entity =>
            {
                entity.HasKey(e => e.Abn);

                entity.Property(e => e.Abn)
                    .HasColumnName("ABN")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.Property(e => e.Email).IsRequired();
            });

            modelBuilder.Entity<PayAgreements>(entity =>
            {
                entity.HasIndex(e => e.CompanyAbn);

                entity.HasIndex(e => e.EmployeeId);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompanyAbn).HasColumnName("CompanyABN");

                entity.Property(e => e.EmployeeId)
                    .IsRequired()
                    .HasColumnName("EmployeeID");

                entity.Property(e => e.PayRate).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.CompanyAbnNavigation)
                    .WithMany(p => p.PayAgreements)
                    .HasForeignKey(d => d.CompanyAbn);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.PayAgreements)
                    .HasForeignKey(d => d.EmployeeId);
            });

            modelBuilder.Entity<Payslips>(entity =>
            {
                entity.HasIndex(e => e.CompanyAbn);

                entity.HasIndex(e => e.EmployeeId);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompanyAbn).HasColumnName("CompanyABN");

                entity.Property(e => e.EmployeeId)
                    .IsRequired()
                    .HasColumnName("EmployeeID");

                entity.Property(e => e.PayYtd).HasColumnName("PayYTD");

                entity.HasOne(d => d.CompanyAbnNavigation)
                    .WithMany(p => p.Payslips)
                    .HasForeignKey(d => d.CompanyAbn);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Payslips)
                    .HasForeignKey(d => d.EmployeeId);
            });

            modelBuilder.Entity<Timesheets>(entity =>
            {
                entity.HasIndex(e => e.CompanyAbn);

                entity.HasIndex(e => e.EmployeeId);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompanyAbn).HasColumnName("CompanyABN");

                entity.Property(e => e.EmployeeId)
                    .IsRequired()
                    .HasColumnName("EmployeeID");

                entity.HasOne(d => d.CompanyAbnNavigation)
                    .WithMany(p => p.Timesheets)
                    .HasForeignKey(d => d.CompanyAbn);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Timesheets)
                    .HasForeignKey(d => d.EmployeeId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
