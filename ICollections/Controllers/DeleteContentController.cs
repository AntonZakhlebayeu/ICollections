using Azure.Storage.Blobs;
using Dropbox.Api;
using ICollections.Data;
using Microsoft.AspNetCore.Mvc;

namespace ICollections.Controllers;

public class DeleteContentController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IConfiguration _configuration;

    public DeleteContentController(ApplicationDbContext context, IConfiguration configuration)
    {
        _db = context;
        _configuration = configuration;
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
                DeleteBlob(item.FileName);
            }

            _db.Items.Remove(item);
        }

        if (objectToDelete!.FileName != "")
        {
            DeleteBlob(objectToDelete.FileName);
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
            DeleteBlob(objectToDelete.FileName);
        }

        var result = _db.Items.Remove(objectToDelete!);

        await _db.SaveChangesAsync();

        return await Task.Run(() => Redirect($"/Home/ViewCollection/{collectionId}"));
    }

    private async void DeleteBlob(string? fileName)
    {
        var connectionString = _configuration.GetConnectionString("BlobStorageConnection");
        var serverClient = new BlobServiceClient(connectionString);
        var containerClient = serverClient.GetBlobContainerClient("images");
        
        var blobClient = containerClient.GetBlobClient(fileName);
        
        await blobClient.DeleteAsync();
    }
}