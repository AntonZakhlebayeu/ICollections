using ICollections.Data.Interfaces;
using ICollections.Models;
using ICollections.Services.Interfaces;

namespace ICollections.Services.Classes;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;

    public CommentService(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task AddComment(Comment comment)
    {
        await _commentRepository.AddAsync(comment);
        await _commentRepository.CommitAsync();
    }

    public int GetCommentIdByText(Comment comment)
    {
        var commentId = _commentRepository.GetSingle(c => c.CommentText == comment.CommentText)!.ItemId;
        return commentId;
    }

    public List<Comment> GetAllComments()
    {
        return _commentRepository.GetAll().ToList();
    }

    public List<Comment> GetCommentsInItem(int itemId)
    {
        return _commentRepository.FindBy(c => c.ItemId == itemId).ToList();
    }
}