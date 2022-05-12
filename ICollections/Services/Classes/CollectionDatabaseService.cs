using ICollections.Data.Interfaces;
using ICollections.Models;
using ICollections.Services.Interfaces;

namespace ICollections.Services.Classes;

public class CollectionDatabaseService : ICollectionDatabase
{
    private readonly ICollectionRepository _collectionRepository;

    public CollectionDatabaseService(ICollectionRepository collectionRepository)
    {
        _collectionRepository = collectionRepository;
    }

    public List<Collection> GetCollectionsByUserId(string userId)
    {
        return _collectionRepository.FindBy(i => i.AuthorId == userId).ToList();
    }

    public async void DeleteCollection(Collection objectToDelete)
    {
        _collectionRepository.Delete(objectToDelete);
        _collectionRepository.Commit();
    }
    
    public Collection GetCollectionByItemId(int id)
    {
        return _collectionRepository.GetSingle(c => c.Id == id)!;
    }

    public Collection GetCollectionByItemIdAsync(int id)
    {
        return _collectionRepository.GetSingleAsync(c => c.Id == id).GetAwaiter().GetResult()!;
    }

    public async void AddCollection(Collection collection)
    {
        _collectionRepository.Add(collection);
        _collectionRepository.Commit();
    }

    public Collection GetCollectionById(int id)
    {
        return _collectionRepository.FindAsync(id).GetAwaiter().GetResult()!;
    }

    public async ValueTask Save()
    {
        await _collectionRepository.CommitAsync();
    }
}