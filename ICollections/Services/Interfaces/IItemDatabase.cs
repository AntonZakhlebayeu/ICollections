using ICollections.Models;
using ICollections.Services.Classes;

namespace ICollections.Services.Interfaces;

public interface IItemDatabase
{
    List<Item> GetItemsByCollectionId(int id);
    Item GetItemById(int id);
    void DeleteItem(Item item);
    ValueTask AddItem(Item item);
    ValueTask Save();
}