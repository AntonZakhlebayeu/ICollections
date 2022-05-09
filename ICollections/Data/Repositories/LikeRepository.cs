using ICollections.Data.Interfaces;
using ICollections.Models;

namespace ICollections.Data.Repositories;

public class LikeRepository : EntityBaseRepository<Like>, ILikeRepository
{
    public LikeRepository(ICollectionDbContext context) : base(context)
    {
    }
}