using System.Diagnostics;
using System.Security.Cryptography;
using Azure.Storage.Blobs;
using ICollections.Data;
using Microsoft.AspNetCore.Mvc;
using ICollections.Models;
using ICollections.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ICollections.Controllers;

public class EditController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IConfiguration _configuration;

    public EditController(ApplicationDbContext context, IConfiguration configuration)
    {
        _db = context;
        _configuration = configuration;
    }
    
    
    [Authorize]
    [HttpGet]
    [Route("/Home/EditCollection/{collectionId:int}")]
    public async Task<ViewResult> EditView(int collectionId)
    {
        ViewBag.collectionId = collectionId;
        
        return await Task.Run(() => View("EditCollection"));
    }
    
    [Authorize]
    [HttpPost]
    [Route("/Home/EditCollection/{collectionId:int}")]
    public async Task<IActionResult> EditCollection(EditCollectionViewModel editCollectionViewModel)
    {
        if (!ModelState.IsValid) return await Task.Run(() => View(editCollectionViewModel));
        
        var editingCollection = _db.Collections.FindAsync(editCollectionViewModel.CollectionId).Result;
        
        var resultingString = "";

        if (Request.Form.Files.Count != 0)
        {
            var file = Request.Form.Files.First();
            resultingString = SaveFileAsync(file).Result;
        }

        editingCollection!.Title = editCollectionViewModel.Title;
        editingCollection!.Description = editCollectionViewModel.Description;
        
        if (Request.Form.Files.Count != 0 && editingCollection!.FileName != "" || 
            Request.Form.Files.Count == 0 && editingCollection!.FileName != "" && editCollectionViewModel.DeleteImage == "true")
        {
            DeleteBlob(editingCollection!.FileName);
            editingCollection!.FileName = resultingString;
        }
        else if (Request.Form.Files.Count != 0 && editingCollection!.FileName == "")
        {
            editingCollection!.FileName = resultingString;
        }

        await _db.SaveChangesAsync();

        return await Task.Run(() => RedirectToAction("ViewCollection", "Home", editCollectionViewModel));
    }
    
    [HttpGet]
    [Route("/Home/EditItem/{collectionId:int}/{itemId:int}")]
    public async Task<ViewResult> EditItem(int collectionId, int itemId)
    {
        ViewBag.collectionId = collectionId;
        ViewBag.itemId = itemId;

        return await Task.Run(() => View("EditItem"));
    }
    
    [Authorize]
    [HttpPost]
    [Route("/Home/EditItem/{collectionId:int}/{itemId:int}")]
    public async Task<IActionResult> EditItem(EditItemViewModel editItemViewModel, int collectionId, int itemId)
    {
        ViewBag.collectionId = collectionId;
        
        if (!ModelState.IsValid) return await Task.Run(() => View(editItemViewModel));

        var editingItem = _db.Items.FindAsync(itemId).Result;
        
        var resultingString = "";
        if (Request.Form.Files.Count != 0)
        {
            var file = Request.Form.Files.First();
            resultingString = SaveFileAsync(file).Result;
        }

        editingItem!.Title = editItemViewModel.Title;
        editingItem!.Description = editItemViewModel.Description;
        editingItem!.Date = editItemViewModel.Date;
        editingItem!.Brand = editItemViewModel.Brand;

        if (Request.Form.Files.Count != 0 && editingItem!.FileName != "" || 
            Request.Form.Files.Count == 0 && editingItem!.FileName != "" && editItemViewModel.DeleteImage == "true")
        {
            DeleteBlob(editingItem.FileName);
            editingItem!.FileName = resultingString;
        }
        else if (Request.Form.Files.Count != 0 && editingItem!.FileName == "")
        {
            editingItem!.FileName = resultingString;
        }

        await _db.SaveChangesAsync();

        return await Task.Run(() => Redirect($"/Home/ViewItem/{collectionId}/{itemId}"));
    }
    
    private async Task PushToCloud(string fileName, string path)
    {
        var connectionString = _configuration.GetConnectionString("BlobStorageConnection");
        
        var serverClient = new BlobServiceClient(connectionString);
        var containerClient = serverClient.GetBlobContainerClient("images");
        var blobClient = containerClient.GetBlobClient(fileName);
        await using var uploadFileStream = System.IO.File.OpenRead(path);
        
        await blobClient.UploadAsync(uploadFileStream, true);
        uploadFileStream.Close();

        System.IO.File.Delete(fileName);
    }

    private static string GetFileName()
    {
        var fileName = Guid.NewGuid().ToString();
        return fileName;
    }

    private async Task<string> SaveFileAsync(IFormFile file)
    {

        var originalFileName = Path.GetFileName(file.FileName);
        var extension = originalFileName.Substring(originalFileName.LastIndexOf('.') + 1, originalFileName.Length - 1 - originalFileName.LastIndexOf('.'));
        var uniqueFileName = GetFileName();

        await using (var stream = System.IO.File.Create(uniqueFileName + '.' + extension))
        {
            await file.CopyToAsync(stream);
        }

        var resultingName = uniqueFileName + '.' + extension;
        await PushToCloud(resultingName, resultingName);

        return resultingName;
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