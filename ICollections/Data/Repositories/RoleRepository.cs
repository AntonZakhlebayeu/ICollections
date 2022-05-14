using ICollections.Data.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ICollections.Data.Repositories;

public class RoleRepository : EntityBaseRepository<IdentityRole>, IRoleRepository
{
    public RoleRepository(CollectionDbContext context) : base(context)
    {
    }
}