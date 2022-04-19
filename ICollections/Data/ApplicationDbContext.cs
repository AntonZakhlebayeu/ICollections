using ICollections.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ICollections.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var adminRole = new IdentityRole("admin");
        var userRole = new IdentityRole("user");
        var superAdminRole = new IdentityRole("super admin");

        modelBuilder.Entity<IdentityRole>()
            .HasData(new IdentityRole[] {superAdminRole, adminRole, userRole});

        base.OnModelCreating(modelBuilder);
    }
}