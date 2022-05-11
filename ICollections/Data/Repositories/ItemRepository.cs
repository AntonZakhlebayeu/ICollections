using ICollections.Data.Interfaces;
using ICollections.Models;

namespace ICollections.Data.Repositories;

public class ItemRepository : EntityBaseRepository<Item>, IItemRepository
{
    public ItemRepository(CollectionDbContext context) : base(context)
    {
    }
}