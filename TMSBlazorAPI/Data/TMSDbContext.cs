using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TMSBlazorAPI.Data;

public partial class TMSDbContext : DbContext
{
    public TMSDbContext()
    {
    }

    public TMSDbContext(DbContextOptions<TMSDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<AdminStatus> AdminStatuses { get; set; }

    public virtual DbSet<AdminsDescription> AdminsDescriptions { get; set; }

    public virtual DbSet<Club> Clubs { get; set; }

    public virtual DbSet<Refreshtoken> Refreshtokens { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=TMSconn");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Admin__3214EC27EAA395FD");

            entity.ToTable("Admin");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.AdmintatusId).HasColumnName("AdmintatusID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Admins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AdminStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__Admin_St__C8EE2043D2D2B897");

            entity.ToTable("Admin_Status");

            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.AdminStatus1)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValueSql("((1))")
                .HasColumnName("AdminStatus");

            entity.HasOne(d => d.Status).WithOne(p => p.AdminStatus)
                .HasForeignKey<AdminStatus>(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Admin_Admin_Status_StatusID");
        });

        modelBuilder.Entity<AdminsDescription>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admins_D__719FE4E88E27D46B");

            entity.ToTable("Admins_Description");

            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.AdminDescription)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.AdminStatusId).HasColumnName("AdminStatusID");
        });

        modelBuilder.Entity<Club>(entity =>
        {
            entity.HasKey(e => e.ClubId).HasName("PK__Club__D35058C71E4333F2");

            entity.ToTable("Club");

            entity.Property(e => e.ClubId).HasColumnName("ClubID");
            entity.Property(e => e.AdminId)
                .HasDefaultValueSql("((1))")
                .HasColumnName("AdminID");
            entity.Property(e => e.ClubName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Clubs).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Refreshtoken>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_tbl_refreshtoken");

            entity.Property(e => e.UserId)
                .ValueGeneratedOnAdd()
                .HasColumnName("UserID");
            entity.Property(e => e.Email)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.Refreshtoken1).HasColumnName("Refreshtoken");
            entity.Property(e => e.TokenId)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithOne(p => p.Refreshtoken)
                .HasForeignKey<Refreshtoken>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Refreshtoken_UserId");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCACCBE633D9");

            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Email)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.MembershipStartDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MembershipStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValueSql("('Non Admin')");
            entity.Property(e => e.Username)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
