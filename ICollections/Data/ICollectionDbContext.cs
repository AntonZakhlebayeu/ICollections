﻿using ICollections.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ICollections.Data;

public sealed class ICollectionDbContext : IdentityDbContext<User>
{
    public DbSet<Collection> Collections { get; set; }
    public DbSet<Item> Items { get; set; } 
    public ICollectionDbContext(DbContextOptions<ICollectionDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureCollectionModel(modelBuilder);
        ConfigureItemModel(modelBuilder);
        ConfigureIdentityRoles(modelBuilder);
        ConfigureLikeModel(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private void ConfigureIdentityRoles(ModelBuilder modelBuilder)
    {
        var adminRole = new IdentityRole("admin");
        var userRole = new IdentityRole("user");
        var superAdminRole = new IdentityRole("super admin");

        modelBuilder.Entity<IdentityRole>()
            .HasData(new IdentityRole[] {superAdminRole, adminRole, userRole});
    }
    
    private void ConfigureCollectionModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Collection>(b =>
        {
            b.ToTable("Collections");
            b.HasKey(p => p.Id);
            b.Property(p => p.AuthorId).HasColumnName("AuthorId").IsRequired();
            b.Property(p => p.Title).HasColumnName("Title").IsRequired();
            b.Property(p => p.Description).HasColumnName("Description").IsRequired();
            b.Property(p => p.Theme).HasColumnName("Theme").IsRequired();
            b.Property(p => p.LastEditDate).HasColumnName("LastEditDate").IsRequired();
            b.Property(p => p.AddDates).HasColumnName("AddDates");
            b.Property(p => p.AddBrands).HasColumnName("AddBrands");
            b.Property(p => p.AddComments).HasColumnName("AddComments");
        });
    }

    private void ConfigureItemModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>(b =>
        {
            b.ToTable("Items");
            b.HasKey(p => p.Id);
            b.Property(p => p.CollectionId).HasColumnName("CollectionId").IsRequired();
            b.Property(p => p.Title).HasColumnName("Title").IsRequired();
            b.Property(p => p.Description).HasColumnName("Description").IsRequired();
            b.Property(p => p.LastEditDate).HasColumnName("LastEditDate").IsRequired();
            b.Property(p => p.Date).HasColumnName("Date");
            b.Property(p => p.Brand).HasColumnName("Brand");
        });
    }
    
    private void ConfigureLikeModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Like>(b =>
        {
            b.ToTable("Like");
            b.HasKey(l => new { l.ItemId, l.UserId });
        });
    }
}