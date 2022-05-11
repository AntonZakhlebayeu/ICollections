using ICollections.Data.Interfaces;
using ICollections.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ICollections.Controllers;

public class DeleteContentController : Controller
{
    private readonly ICollectionRepository _collectionRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IDeleteBlob _deleteBlob;

    public DeleteContentController(IDeleteBlob deleteBlob, IItemRepository itemRepository, ICollectionRepository collectionRepository)
    {
        _deleteBlob = deleteBlob;
        _itemRepository = itemRepository;
        _collectionRepository = collectionRepository;
    }
    
    [Route("/Home/ViewItem/{collectionId}/DeleteCollection")]
    public async Task<IActionResult> DeleteCollection(int collectionId)
    {
        var objectToDelete = _collectionRepository.FindAsync(collectionId).Result;
        
        var itemsToDelete = _itemRepository.FindBy(i => i.CollectionId == objectToDelete!.Id);

        foreach (var item in itemsToDelete)
        {
            if (item.FileName != "")
            {
                _deleteBlob.DeleteBlob(item.FileName);
            }
            
            _itemRepository.Delete(item);
        }

        if (objectToDelete!.FileName != "")
        {
            _deleteBlob.DeleteBlob(objectToDelete.FileName);
        }
        
        _collectionRepository.Delete(objectToDelete);
        
        await _collectionRepository.CommitAsync();
        await _itemRepository.CommitAsync();

        return await Task.Run(() => RedirectToAction("Profile", "Home"));
    }
    
    [Route("/Home/ViewItem/{collectionId}/{itemId:int}/DeleteItem")]
    public async Task<IActionResult> DeleteItem(int itemId, int collectionId)
    {
        var objectToDelete = _itemRepository.FindAsync(itemId).Result;
        
        if (objectToDelete!.FileName != "")
        {
            _deleteBlob.DeleteBlob(objectToDelete.FileName);
        }
        
        _itemRepository.Delete(objectToDelete);
        
        await _itemRepository.CommitAsync();

        return await Task.Run(() => Redirect($"/Home/ViewCollection/{collectionId}"));
    }
}