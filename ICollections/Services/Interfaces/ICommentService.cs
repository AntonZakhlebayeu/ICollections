using ICollections.Models;

namespace ICollections.Services.Interfaces;

public interface ICommentService
{
    Task AddComment(Comment comment);
    int GetCommentIdByText(Comment comment);
    List<Comment> GetAllComments();
    List<Comment> GetCommentsInItem(int itemId);
}