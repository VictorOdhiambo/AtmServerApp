using System;
using System.Collections.Generic;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RepositoryLayer
{
    public partial class UpesiAtmDBContext : DbContext
    {
        public UpesiAtmDBContext()
        {
        }

        public UpesiAtmDBContext(DbContextOptions<UpesiAtmDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ACCOUNT> ACCOUNTs { get; set; } = null!;
        public virtual DbSet<ATM> ATMs { get; set; } = null!;
        public virtual DbSet<AUDITTRAIL> AUDITTRAILs { get; set; } = null!;
        public virtual DbSet<STATUS> STATUSes { get; set; } = null!;
        public virtual DbSet<USER> USERs { get; set; } = null!;

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//                optionsBuilder.UseSqlServer("Server=DITLPT036\\SQLEXPRESS01;Initial Catalog=UPESI;Persist Security Info=False;User ID=sa;Password=pass;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=3600;");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ACCOUNT>(entity =>
            {
                entity.HasOne(d => d.STATUS)
                    .WithMany(p => p.ACCOUNTs)
                    .HasForeignKey(d => d.STATUSID)
                    .HasConstraintName("FK__ACCOUNT__STATUSI__3D5E1FD2");

                entity.HasOne(d => d.USER)
                    .WithMany(p => p.ACCOUNTs)
                    .HasForeignKey(d => d.USERID)
                    .HasConstraintName("FK__ACCOUNT__USERID__3C69FB99");
            });

            modelBuilder.Entity<ATM>(entity =>
            {
                entity.HasOne(d => d.STATUS)
                    .WithMany(p => p.ATMs)
                    .HasForeignKey(d => d.STATUSID)
                    .HasConstraintName("FK__ATM__STATUSID__403A8C7D");
            });

            modelBuilder.Entity<AUDITTRAIL>(entity =>
            {
                entity.HasOne(d => d.USER)
                    .WithMany(p => p.AUDITTRAILs)
                    .HasForeignKey(d => d.USERID)
                    .HasConstraintName("FK__AUDITTRAI__USERI__4316F928");
            });

            modelBuilder.Entity<USER>(entity =>
            {
                entity.HasOne(d => d.STATUS)
                    .WithMany(p => p.USERs)
                    .HasForeignKey(d => d.STATUSID)
                    .HasConstraintName("FK__USER__STATUSID__398D8EEE");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
