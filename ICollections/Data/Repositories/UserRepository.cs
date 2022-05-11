using ICollections.Data.Interfaces;
using ICollections.Models;

namespace ICollections.Data.Repositories;

public class UserRepository : EntityBaseRepository<User>, IUserRepository
{
    public UserRepository(CollectionDbContext context) : base(context)
    {
    }
}