using System;
using Adminpanel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Adminpanel.DataAccess
{
    public partial class DB_SchoolMgmtContext : DbContext
    {
        public DB_SchoolMgmtContext()
        {
        }

        public DB_SchoolMgmtContext(DbContextOptions<DB_SchoolMgmtContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblAboutUsMaster> TblAboutUsMaster { get; set; }
        public virtual DbSet<TblAdminLogin> TblAdminLogin { get; set; }
        public virtual DbSet<TblBannerMaster> TblBannerMaster { get; set; }
        public virtual DbSet<TblCourseMaster> TblCourseMaster { get; set; }
        public virtual DbSet<TblGalleryMaster> TblGalleryMaster { get; set; }
        public virtual DbSet<BannerDetail>Get_BannerDetails { get; set; }
        public virtual DbSet<AboutUsDetails>Get_AboutUsDetails { get; set; }
        public virtual DbSet<CourseDetails>Get_CourseDetails { get; set; }
        public virtual DbSet<GalleryDetail>Get_GalleryDetails { get; set; }
      

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=PANKAJ\\SQLEXPRESS;Initial Catalog=DB_SchoolMgmt;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblAboutUsMaster>(entity =>
            {
                entity.ToTable("Tbl_AboutUsMaster");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AboutUsHeading).HasMaxLength(150);

                entity.Property(e => e.AboutsFileName).HasMaxLength(100);

                entity.Property(e => e.AboutsFilePath).HasMaxLength(100);

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.IsDelete).HasColumnName("isDelete");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblAdminLogin>(entity =>
            {
                entity.ToTable("Tbl_AdminLogin");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AddedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.UserName).HasMaxLength(100);

                entity.Property(e => e.UserPassword).HasMaxLength(20);
            });

            modelBuilder.Entity<TblBannerMaster>(entity =>
            {
                entity.ToTable("Tbl_BannerMaster");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.BannerHeading).HasMaxLength(150);

                entity.Property(e => e.BannerImage).HasMaxLength(100);

                entity.Property(e => e.BannerPath).HasMaxLength(100);

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.IsDelete).HasColumnName("isDelete");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblCourseMaster>(entity =>
            {
                entity.ToTable("Tbl_CourseMaster");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.CourseFileName).HasMaxLength(100);

                entity.Property(e => e.CourseFilePath).HasMaxLength(100);

                entity.Property(e => e.CourseName).HasMaxLength(150);

                entity.Property(e => e.CoursePunchLine).HasMaxLength(50);

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.IsDelete).HasColumnName("isDelete");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblGalleryMaster>(entity =>
            {
                entity.ToTable("Tbl_GalleryMaster");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.GalleryFileName).HasMaxLength(100);

                entity.Property(e => e.GalleryFilePath).HasMaxLength(100);

                entity.Property(e => e.GalleryPunchLine).HasMaxLength(50);

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.IsDelete).HasColumnName("isDelete");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
