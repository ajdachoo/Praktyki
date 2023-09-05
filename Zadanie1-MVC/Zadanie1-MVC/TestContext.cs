using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Zadanie1_MVC.Models;

namespace Zadanie1_MVC;

public partial class TestContext : DbContext
{
    public virtual DbSet<Klient> Klienci { get; set; }

    public TestContext(DbContextOptions<TestContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Klient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Klienci_pkey");

            entity.ToTable("Klienci");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Pesel)
                .HasMaxLength(11)
                .HasColumnName("PESEL");
            entity.Property(e => e.Surname).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
