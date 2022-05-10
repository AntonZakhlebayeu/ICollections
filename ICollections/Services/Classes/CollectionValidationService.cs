using ICollections.Data.Interfaces;
using ICollections.Services.Interfaces;

namespace ICollections.Services.Classes;

public class CollectionValidationService : ICollectionValidation
{
    private readonly ICollectionRepository _collectionRepository;

    public CollectionValidationService(ICollectionRepository collectionRepository)
    {
        _collectionRepository = collectionRepository;
    }

    public bool IsCollectionNull(int collectionId)
    {
        var collection = _collectionRepository.GetSingleAsync(c => c.Id == collectionId).Result;

        return collection == null;
    }
}