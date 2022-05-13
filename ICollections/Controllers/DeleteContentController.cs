using ICollections.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ICollections.Controllers;

public class DeleteContentController : Controller
{
    private readonly IDeleteBlob _deleteBlob;
    private readonly ICollectionService _collectionService;
    private readonly IItemManager _itemManager;

    public DeleteContentController(IDeleteBlob deleteBlob, ICollectionService collectionService, IItemManager itemManager)
    {
        _deleteBlob = deleteBlob;
        _collectionService = collectionService;
        _itemManager = itemManager;
    }
    
    [Route("/Home/ViewItem/{collectionId:int}/DeleteCollection")]
    public async Task<IActionResult> DeleteCollection(int collectionId)
    {
        var objectToDelete = _collectionService.GetCollectionById(collectionId);

        var itemsToDelete = _itemManager.GetItemsByCollectionId(collectionId);

        foreach (var item in itemsToDelete)
        {
            if (item.FileName != "")
            {
                _deleteBlob.DeleteBlob(item.FileName);
            }
            
            _itemManager.DeleteItem(item);
        }

        if (objectToDelete.FileName != "")
        {
            _deleteBlob.DeleteBlob(objectToDelete.FileName);
        }
        
        _collectionService.DeleteCollection(objectToDelete);

        return await Task.Run(() => RedirectToAction("Profile", "Home"));
    }
    
    [Route("/Home/ViewItem/{collectionId:int}/{itemId:int}/DeleteItem")]
    public async Task<IActionResult> DeleteItem(int itemId, int collectionId)
    {
        var objectToDelete = _itemManager.GetItemById(itemId);
        
        if (objectToDelete.FileName != "")
        {
            _deleteBlob.DeleteBlob(objectToDelete.FileName);
        }
        
        _itemManager.DeleteItem(objectToDelete);

        return await Task.Run(() => Redirect($"/Home/ViewCollection/{collectionId}"));
    }
}