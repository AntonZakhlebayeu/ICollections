using ICollections.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ICollections.Controllers;

public class DeleteContentController : Controller
{
    private readonly IDeleteBlob _deleteBlob;
    private readonly ICollectionDatabase _collectionDatabase;
    private readonly IItemDatabase _itemDatabase;

    public DeleteContentController(IDeleteBlob deleteBlob, ICollectionDatabase collectionDatabase, IItemDatabase itemDatabase)
    {
        _deleteBlob = deleteBlob;
        _collectionDatabase = collectionDatabase;
        _itemDatabase = itemDatabase;
    }
    
    [Route("/Home/ViewItem/{collectionId}/DeleteCollection")]
    public async Task<IActionResult> DeleteCollection(int collectionId)
    {
        var objectToDelete = _collectionDatabase.GetCollectionById(collectionId);

        var itemsToDelete = _itemDatabase.GetItemsByCollectionId(collectionId);

        foreach (var item in itemsToDelete)
        {
            if (item.FileName != "")
            {
                _deleteBlob.DeleteBlob(item.FileName);
            }
            
            _itemDatabase.DeleteItem(item);
        }

        if (objectToDelete.FileName != "")
        {
            _deleteBlob.DeleteBlob(objectToDelete.FileName);
        }
        
        _collectionDatabase.DeleteCollection(objectToDelete);

        return await Task.Run(() => RedirectToAction("Profile", "Home"));
    }
    
    [Route("/Home/ViewItem/{collectionId}/{itemId:int}/DeleteItem")]
    public async Task<IActionResult> DeleteItem(int itemId, int collectionId)
    {
        var objectToDelete = _itemDatabase.GetItemById(itemId);
        
        if (objectToDelete.FileName != "")
        {
            _deleteBlob.DeleteBlob(objectToDelete.FileName);
        }
        
        _itemDatabase.DeleteItem(objectToDelete);

        return await Task.Run(() => Redirect($"/Home/ViewCollection/{collectionId}"));
    }
}