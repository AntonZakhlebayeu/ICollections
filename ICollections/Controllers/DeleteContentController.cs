using Dropbox.Api;
using ICollections.Constants;
using ICollections.Data;
using Microsoft.AspNetCore.Mvc;

namespace ICollections.Controllers;

public class DeleteContentController : Controller
{
    private readonly ApplicationDbContext _db;

    public DeleteContentController(ApplicationDbContext context)
    {
        _db = context;
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
                using (var dbx = new DropboxClient(AccessDropBoxConstants.GetToken()))
                {
                    item.FileUrl += "?dl=0";
                    await dbx.Files.DeleteV2Async(AccessDropBoxConstants.Folder + "/" + item.FileName);
                }
            }

            _db.Items.Remove(item);
        }

        if (objectToDelete!.FileName != "")
        {
            using (var dbx = new DropboxClient(AccessDropBoxConstants.GetToken()))
            {
                objectToDelete.FileUrl += "?dl=0";
                await dbx.Files.DeleteV2Async(AccessDropBoxConstants.Folder + "/" + objectToDelete.FileName);
            }
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
            using (var dbx = new DropboxClient(AccessDropBoxConstants.GetToken()))
            {
                objectToDelete.FileUrl += "?dl=0";
                await dbx.Files.DeleteV2Async(AccessDropBoxConstants.Folder + "/" + objectToDelete.FileName);
            }
        }

        var result = _db.Items.Remove(objectToDelete!);

        await _db.SaveChangesAsync();

        return await Task.Run(() => Redirect($"/Home/ViewCollection/{collectionId}"));
    }
}