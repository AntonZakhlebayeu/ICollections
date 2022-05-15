using ICollections.Data.Interfaces;
using ICollections.Models;

namespace ICollections.Data.Repositories;

public class TagRepository : EntityBaseRepository<Tag>, ITagRepository
{
    public TagRepository(CollectionDbContext context) : base(context)
    {
    }
}