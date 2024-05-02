using System;
using System.Collections.Generic;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Common;

public partial class DigitalKhataDbContext : DbContext
{
    public DigitalKhataDbContext()
    {
    }

    public DigitalKhataDbContext(DbContextOptions<DigitalKhataDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.ExpenseId).HasName("PK__Expenses__1445CFD396AB20DD");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);

            entity.HasOne(d => d.PayeeUser).WithMany(p => p.ExpensePayeeUsers)
                .HasForeignKey(d => d.PayeeUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PayeeUserId");

            entity.HasOne(d => d.PayerUser).WithMany(p => p.ExpensePayerUsers)
                .HasForeignKey(d => d.PayerUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PayerUserId");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C0790E53A");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E45F92871E").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105344149212D").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Fullname).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
