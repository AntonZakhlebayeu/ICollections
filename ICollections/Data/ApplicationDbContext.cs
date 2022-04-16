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
        Database.EnsureCreated();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        var adminRole = new IdentityRole("admin");
        var userRole = new IdentityRole("user");

        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole[] { adminRole, userRole });

        base.OnModelCreating(modelBuilder);
    }
}