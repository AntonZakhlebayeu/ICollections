using ICollections.Data.Interfaces;
using ICollections.Services.Interfaces;

namespace ICollections.Services.Classes;

public class LikeValidationService : ILikeValidation
{
    private readonly IUserRepository _userRepository;
    private readonly ICollectionRepository _collectionRepository;
    private readonly IItemRepository _itemRepository;

    public LikeValidationService(IUserRepository userRepository, ICollectionRepository collectionRepository, IItemRepository itemRepository)
    {
        _userRepository = userRepository;
        _collectionRepository = collectionRepository;
        _itemRepository = itemRepository;
    }

    public bool IsUserOwner(string userEmail, int collectionId)
    {
        return _userRepository.GetSingleAsync(u => u.UserName == userEmail).Result!.Id ==
            _collectionRepository.GetSingleAsync(c => c.Id == collectionId).Result!.AuthorId;
    }
}