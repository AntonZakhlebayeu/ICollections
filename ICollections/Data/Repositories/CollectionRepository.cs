using ICollections.Data.Interfaces;
using ICollections.Models;

namespace ICollections.Data.Repositories;

public class CollectionRepository : EntityBaseRepository<Collection>, ICollectionRepository
{
    public CollectionRepository(ICollectionDbContext context) : base(context)
    {
    }
}