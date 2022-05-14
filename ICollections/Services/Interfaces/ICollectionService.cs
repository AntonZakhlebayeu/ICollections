using ICollections.Models;

namespace ICollections.Services.Interfaces;

public interface ICollectionService
{
    List<Collection> GetCollectionsByUserId(string userId);
    void DeleteCollection(Collection objectToDelete);
    Collection GetCollectionByItemId(int id);
    Collection GetCollectionByItemIdAsync(int id);
    void AddCollection(Collection collection);
    Collection GetCollectionById(int id);
    ValueTask Save();
    List<Collection> GetAll();
}