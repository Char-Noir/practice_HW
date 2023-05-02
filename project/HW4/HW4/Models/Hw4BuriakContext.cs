using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HW4.Models;

public partial class Hw4BuriakContext : DbContext
{
    public Hw4BuriakContext()
    {
    }

    public Hw4BuriakContext(DbContextOptions<Hw4BuriakContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=HW_4_Buriak;Trusted_Connection=True;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.HasIndex(e => e.RoleName, "role_name_unique").IsUnique();

            entity.Property(e => e.RoleId)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd()
                .HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("role_name");
        });
        modelBuilder.Entity<Role>().HasData(new Role() { RoleId = 1, RoleName="Developer" });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_user");

            entity.ToTable("User");

            entity.HasIndex(e => e.UserEmail, "user_email_unique").IsUnique();

            entity.Property(e => e.UserId)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd()
                .HasColumnName("user_id");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(255)
                .IsFixedLength()
                .HasColumnName("user_email");
            entity.Property(e => e.UserName)
                .HasMaxLength(60)
                .IsFixedLength()
                .HasColumnName("user_name");
            entity.Property(e => e.UserRole).HasColumnName("user_role");

            entity.HasOne(d => d.UserRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_user_role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
