using ICollections.Data.Interfaces;
using ICollections.Models;

namespace ICollections.Data.Repositories;

public class CommentRepository : EntityBaseRepository<Comment>, ICommentRepository
{
    public CommentRepository(CollectionDbContext context) : base(context)
    {
    }
}