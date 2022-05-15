using ICollections.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ICollections.Data;

public sealed class CollectionDbContext : IdentityDbContext<User>
{
    private readonly IConfiguration _configuration;
    
    public DbSet<Collection> Collections { get; set; } = null!;
    public DbSet<Item> Items { get; set; } = null!;

    public CollectionDbContext(DbContextOptions<CollectionDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;

        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(_configuration.GetConnectionString("MySqlConnectionString"),
           new MySqlServerVersion(new Version(8, 0, 27)));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureCollectionModel();
        modelBuilder.ConfigureItemModel();
        modelBuilder.ConfigureIdentityRoles();
        modelBuilder.ConfigureLikeModel();
        modelBuilder.ConfigureCommentModel();
        modelBuilder.ConfigureTagModel();

        base.OnModelCreating(modelBuilder);
    }
}