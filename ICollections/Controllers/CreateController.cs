using System.Text;
using Dropbox.Api;
using Dropbox.Api.Files;
using ICollections.Constants;
using ICollections.Data;
using Microsoft.AspNetCore.Mvc;
using ICollections.Models;
using ICollections.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ICollections.Controllers;

public class CreateController : Controller
{
    private readonly ApplicationDbContext _db;

    public CreateController(ApplicationDbContext context)
    {
        _db = context;
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ViewResult> CreateView()
    {
        return await Task.Run(() => View("CreateCollection"));
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateCollection(CollectionViewModel collectionViewModel)
    {
        if (!ModelState.IsValid || collectionViewModel.Theme == "null") return await Task.Run(() => View(collectionViewModel));

        var resultingName = "";
        
        if (Request.Form.Files.Count != 0)
        {
            var file = Request.Form.Files.First();
            resultingName = SaveFileAsync(file).Result;
        }

        var newCollection = new Collection
        {
            AuthorId = collectionViewModel.AuthorId, Title = collectionViewModel.Title,
            Description = collectionViewModel.Description, Theme = collectionViewModel.Theme,
            AddDates = collectionViewModel.IncludeDate, AddBrands = collectionViewModel.IncludeBrand,
            AddComments = collectionViewModel.IncludeComments, LastEditDate = DateTime.UtcNow.AddHours(3).ToString("MM/dd/yyyy H:mm"),
            FileName = resultingName,
        };

        await _db.Collections.AddAsync(newCollection);

        await _db.SaveChangesAsync();

        return await Task.Run(() => RedirectToAction("ViewCollection", "Home", newCollection));
    }

    [HttpGet]
    [Route("/Home/AddItem/{collectionId:int}")]
    public async Task<ViewResult> AddItem(int collectionId)
    {
        ViewBag.collectionId = collectionId;

        return await Task.Run(() => View("AddItem"));
    }
    
    [Authorize]
    [HttpPost]
    [Route("/Home/AddItem/{collectionId:int}")]
    public async Task<IActionResult> AddItem(ItemViewModel itemViewModel, int collectionId)
    {
        ViewBag.collectionId = collectionId;
        
        if (!ModelState.IsValid) return await Task.Run(() => View(itemViewModel));

        var newItem = new Item
        {
            CollectionId = itemViewModel.CollectionId, Title = itemViewModel.Title,
            Description = itemViewModel.Description, LastEditDate = DateTime.UtcNow.AddHours(3).ToString("MM/dd/yyyy H:mm"),
            Date = itemViewModel.Date, Brand = itemViewModel.Brand
        };

        var currentCollection = _db.Collections.FirstOrDefaultAsync(c => c.CollectionId == newItem.CollectionId).Result;

        currentCollection!.CollectionItems!.Append(newItem);

        await _db.Items.AddAsync(newItem);

        await _db.SaveChangesAsync();

        return await Task.Run(() => RedirectToAction("ViewCollection", "Home", currentCollection));
    }
    
    private static string PushToCloud(string fileName, string path)
    {
        using var uploadFileStream = System.IO.File.OpenRead(path);

        string url;
        
        using (var dbx = new DropboxClient(AccessDropBoxConstants.GetToken()))
        {
            using (var mem = new MemoryStream(System.IO.File.ReadAllBytes(uploadFileStream.Name)))
            {
                var updated = dbx.Files.UploadAsync(AccessDropBoxConstants.Folder + "/" + fileName, WriteMode.Overwrite.Instance, body: mem);
                updated.Wait();
                var tx = dbx.Sharing.CreateSharedLinkWithSettingsAsync(AccessDropBoxConstants.Folder  + "/" + fileName);
                tx.Wait();

                url = tx.Result.Url;
                url = url.TrimEnd(AccessDropBoxConstants.ToCut);
            }
        }
        uploadFileStream.Close();
        System.IO.File.Delete(fileName);

        return url;
    }
    
    private static string GetFileName()
    {
        var fileName = Guid.NewGuid().ToString();
        return fileName;
    }

    private static async Task<string> SaveFileAsync(IFormFile file)
    {

        var originalFileName = Path.GetFileName(file.FileName);
        var extension = originalFileName.Substring(originalFileName.LastIndexOf('.') + 1, originalFileName.Length - 1 - originalFileName.LastIndexOf('.'));
        var uniqueFileName = GetFileName();

        using (var stream = System.IO.File.Create(uniqueFileName + '.' + extension))
        {
            await file.CopyToAsync(stream);
        }

        var resultingName = uniqueFileName + '.' + extension;

        resultingName = PushToCloud(resultingName, resultingName);

        return resultingName;
    }
}