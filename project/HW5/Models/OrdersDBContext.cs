using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HW5.Models;

public partial class OrdersDBContext : DbContext
{
    public OrdersDBContext()
    {
    }

    public OrdersDBContext(DbContextOptions<OrdersDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Analysis> Analyses { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Analysis>(entity =>
        {
            entity.HasKey(e => e.AnId).HasName("PK__Analysis__831DABF3D66BED17");

            entity.ToTable("Analysis");

            entity.Property(e => e.AnId).HasColumnName("an_id");
            entity.Property(e => e.AnCost)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("an_cost");
            entity.Property(e => e.AnGroup).HasColumnName("an_group");
            entity.Property(e => e.AnName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("an_name");
            entity.Property(e => e.AnPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("an_price");

            entity.HasOne(d => d.AnGroupNavigation).WithMany(p => p.Analyses)
                .HasForeignKey(d => d.AnGroup)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Analysis__an_gro__412EB0B6");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.GrId).HasName("PK__Groups__2BC0F88EFCC9B256");

            entity.Property(e => e.GrId).HasColumnName("gr_id");
            entity.Property(e => e.GrName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("gr_name");
            entity.Property(e => e.GrTemp)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("gr_temp");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrdId).HasName("PK__Orders__3214EC276B84A24E");

            entity.Property(e => e.OrdId).HasColumnName("ord_id");
            entity.Property(e => e.OrdAn).HasColumnName("ord_an");
            entity.Property(e => e.OrdDatetime)
                .HasColumnType("datetime")
                .HasColumnName("ord_datetime");

            entity.HasOne(d => d.OrdAnNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrdAn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__ord_an__440B1D61");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
