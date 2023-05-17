using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HW78.Models;

public partial class ExpensesDbContext : DbContext
{
    public ExpensesDbContext()
    {
    }

    public ExpensesDbContext(DbContextOptions<ExpensesDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Expense> Expenses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCategory);

            entity.ToTable("Category");

            entity.Property(e => e.IdCategory).ValueGeneratedOnAdd().HasColumnName("id_category");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("is_active");
            entity.Property(e => e.IsVisible)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("is_visible");
            entity.Property(e => e.NameCategory)
                .HasMaxLength(64)
                .IsFixedLength()
                .HasColumnName("name_category");
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.IdExpense);

            entity.ToTable("Expense");

            entity.Property(e => e.IdExpense)
                .ValueGeneratedOnAdd()
                .HasColumnName("id_expense");
            entity.Property(e => e.Commentary)
                .HasColumnType("text")
                .HasColumnName("commentary");
            entity.Property(e => e.CostExpense).HasColumnName("cost_expense");
            entity.Property(e => e.FkCategory).HasColumnName("fk_category");
            entity.Property(e => e.IsVisible)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("is_visible");

            entity.HasOne(d => d.FkCategoryNavigation).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.FkCategory)
                .OnDelete(DeleteBehavior.ClientNoAction)
                .HasConstraintName("FK_Expence_Category");

        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
