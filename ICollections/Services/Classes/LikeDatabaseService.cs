using ICollections.Data.Interfaces;
using ICollections.Models;
using ICollections.Services.Interfaces;

namespace ICollections.Services.Classes;

public class LikeDatabaseService : ILikeDatabase
{
    private readonly ILikeRepository _likeRepository;

    public LikeDatabaseService(ILikeRepository likeRepository)
    {
        _likeRepository = likeRepository;
    }

    public List<Like> GetLikesByItemId(int id)
    {
        return _likeRepository.FindBy(l => l.ItemId == id).ToList();
    }

    public void AddLike(Like like)
    {
        _likeRepository.Add(like);
        _likeRepository.Commit();
    }

    public void DeleteLike(Like like)
    {
        _likeRepository.Delete(like);
        _likeRepository.Commit();
    }

    public Like? IsLikeExists(string userId, int itemId)
    {
        return _likeRepository.GetSingle(l => l.ItemId == itemId && l.UserId == userId);
    }
}