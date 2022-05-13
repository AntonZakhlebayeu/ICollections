using ICollections.Data.Interfaces;
using ICollections.Models;
using ICollections.Services.Interfaces;

namespace ICollections.Services.Classes;

public class ItemService : IItemManager
{
    private readonly IItemRepository _itemRepository;

    public ItemService(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public List<Item> GetItemsByCollectionId(int id)
    {
        return _itemRepository.FindBy(i => i.CollectionId == id).ToList();
    }

    public Item GetItemById(int id)
    {
        return _itemRepository.FindAsync(id).GetAwaiter().GetResult()!;
    }

    public void DeleteItem(Item item)
    {
        _itemRepository.Delete(item);
        _itemRepository.Commit();
    }

    public async ValueTask AddItem(Item item)
    {
        await _itemRepository.AddAsync(item);
        await _itemRepository.CommitAsync();
    }

    public async ValueTask Save()
    {
        await _itemRepository.CommitAsync();
    }
}