using ICollections.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ICollections.Data;

public static class ConfigureDbEntities
{
    public static void ConfigureIdentityRoles(this ModelBuilder modelBuilder)
    {
        var adminRole = new IdentityRole("admin")
        {
            NormalizedName = "admin".ToUpper()
        };
        var userRole = new IdentityRole("user")
        {
            NormalizedName = "user".ToUpper()
        };
        var superAdminRole = new IdentityRole("super admin")
        {
            NormalizedName = "super admin".ToUpper()
        };

        modelBuilder.Entity<IdentityRole>(r =>
        {
            r.HasData(superAdminRole, adminRole, userRole);
        });

    }
    
    public static void ConfigureCollectionModel(this ModelBuilder modelBuilder)
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

    public static void ConfigureItemModel(this ModelBuilder modelBuilder)
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
    
    public static void ConfigureLikeModel(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Like>(b =>
        {
            b.ToTable("Like");
            b.HasKey(l => new { l.ItemId, l.UserId });
        });
    }

    public static void ConfigureCommentModel(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(b =>
        {
            b.ToTable("Comment");
            b.HasKey(c => c.Id);
            b.Property(c => c.UserNickName).HasColumnName("UserNickName");
            b.Property(c => c.ItemId).HasColumnName("ItemId");
            b.Property(c => c.CommentText).HasColumnName("CommentText").IsRequired();
            b.Property(c => c.CommentWhen).HasColumnName("CommentWhen").IsRequired();
        });
    }
    
    public static void ConfigureTagModel(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tag>(b =>
        {
            b.ToTable("Tag");
            b.HasKey(t => t.Id);
            b.Property(t => t.TagText).IsRequired();
        });
    }
}