using Azure.Storage.Blobs;
using Dropbox.Api;
using ICollections.Data;
using ICollections.Services;
using Microsoft.AspNetCore.Mvc;

namespace ICollections.Controllers;

public class DeleteContentController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IConfiguration _configuration;
    private readonly IDeleteBlob _deleteBlob;

    public DeleteContentController(ApplicationDbContext context, IConfiguration configuration, IDeleteBlob deleteBlob)
    {
        _db = context;
        _configuration = configuration;
        _deleteBlob = deleteBlob;
    }
    
    [Route("/Home/ViewItem/{collectionId}/DeleteCollection")]
    public async Task<IActionResult> DeleteCollection(int collectionId)
    {
        var objectToDelete = _db.Collections.FindAsync(collectionId).Result;

        var itemsToDelete = _db.Items.Where(i => i.CollectionId == objectToDelete!.CollectionId);

        foreach (var item in itemsToDelete)
        {
            if (item!.FileName != "")
            {
                _deleteBlob.DeleteBlob(item.FileName);
            }

            _db.Items.Remove(item);
        }

        if (objectToDelete!.FileName != "")
        {
            _deleteBlob.DeleteBlob(objectToDelete.FileName);
        }

        _db.Collections.Remove(objectToDelete!);

        await _db.SaveChangesAsync();

        return await Task.Run(() => RedirectToAction("Profile", "Home"));
    }
    
    [Route("/Home/ViewItem/{collectionId}/{itemId:int}/DeleteItem")]
    public async Task<IActionResult> DeleteItem(int itemId, int collectionId)
    {
        var objectToDelete = _db.Items.FindAsync(itemId).Result;
        
        if (objectToDelete!.FileName != "")
        {
            _deleteBlob.DeleteBlob(objectToDelete.FileName);
        }

        var result = _db.Items.Remove(objectToDelete!);

        await _db.SaveChangesAsync();

        return await Task.Run(() => Redirect($"/Home/ViewCollection/{collectionId}"));
    }
}