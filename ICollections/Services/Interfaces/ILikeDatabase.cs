using ICollections.Models;

namespace ICollections.Services.Interfaces;

public interface ILikeDatabase
{
    List<Like> GetLikesByItemId(int id);
    void AddLike(Like like);
    void DeleteLike(Like like);
    Like? IsLikeExists(string userId, int itemId);
}