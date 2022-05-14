using HigLabo.Core;
using ICollections.Data.Interfaces;
using ICollections.Models;
using ICollections.Services.Interfaces;

namespace ICollections.Services.Classes;

public class ItemService : IItemService
{
    private readonly IItemRepository _itemRepository;
    private readonly ICollectionRepository _collectionRepository;

    public ItemService(IItemRepository itemRepository, ICollectionRepository collectionRepository)
    {
        _itemRepository = itemRepository;
        _collectionRepository = collectionRepository;
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

    public List<Item> FullTextSearch(string searchString)
    {
        var resultItems = new List<Item>();

        if (searchString.IsNullOrEmpty()) return resultItems;
        
        resultItems = _itemRepository.FullTextSearch(searchString).ToList();

        var itemsInCollection = _collectionRepository.FullTextSearch(searchString).ToList();

        foreach (var collection in itemsInCollection)
        {
            collection.CollectionItems = _itemRepository.FindBy(c => c.CollectionId == collection.Id).ToList();

            foreach (var item in collection.CollectionItems!.Where(item =>
                         !resultItems.Contains(item)))
            {
                resultItems.Add(item);
            }
        }

        return resultItems;
    }

    public List<Item> GetAll()
    {
        return _itemRepository.GetAll().ToList();
    }
}