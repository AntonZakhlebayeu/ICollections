using ICollections.Data.Interfaces;
using ICollections.Services.Interfaces;

namespace ICollections.Services.Classes;

public class ItemValidationService : IItemValidation
{
    private readonly IItemRepository _itemRepository;

    public ItemValidationService(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public bool IsItemNull(int itemId)
    {
        var item = _itemRepository.GetSingleAsync(i => i.Id == itemId).Result;

        return item == null;
    }
}