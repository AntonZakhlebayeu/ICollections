using Azure.Storage.Blobs;
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
    private readonly IConfiguration _configuration;

    public CreateController(ApplicationDbContext context, IConfiguration configuration)
    {
        _db = context;
        _configuration = configuration;
    }
    
    [Authorize]
    [HttpGet]
    public ViewResult CreateView()
    {
        return View("CreateCollection");
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateCollection(CollectionViewModel collectionViewModel)
    {
        if (!ModelState.IsValid || collectionViewModel.Theme == "null") return await Task.Run(() => View(collectionViewModel));

        var resultingString = "";

        if (Request.Form.Files.Count != 0)
        {
            var file = Request.Form.Files.First();
            resultingString = SaveFileAsync(file).Result;
        }

        var newCollection = new Collection
        {
            AuthorId = collectionViewModel.AuthorId, Title = collectionViewModel.Title,
            Description = collectionViewModel.Description, Theme = collectionViewModel.Theme,
            AddDates = collectionViewModel.IncludeDate, AddBrands = collectionViewModel.IncludeBrand,
            AddComments = collectionViewModel.IncludeComments, LastEditDate = DateTime.UtcNow.AddHours(3).ToString("MM/dd/yyyy H:mm"),
            FileName = resultingString,
        };

        await _db.Collections.AddAsync(newCollection);

        await _db.SaveChangesAsync();

        return await Task.Run(() => RedirectToAction("ViewCollection", "Home", newCollection));
    }

    [HttpGet]
    [Route("/Home/AddItem/{collectionId:int}")]
    public ViewResult AddItem(int collectionId)
    {
        ViewBag.collectionId = collectionId;

        return View("AddItem");
    }
    
    [Authorize]
    [HttpPost]
    [Route("/Home/AddItem/{collectionId:int}")]
    public async Task<IActionResult> AddItem(ItemViewModel itemViewModel, int collectionId)
    {
        ViewBag.collectionId = collectionId;
        
        if (!ModelState.IsValid) return await Task.Run(() => View(itemViewModel));

        var resultingStrings = ""; 

        if (Request.Form.Files.Count != 0)
        {
            var file = Request.Form.Files.First();
            resultingStrings = SaveFileAsync(file).Result;
        }

        
        var newItem = new Item
        {
            CollectionId = itemViewModel.CollectionId, Title = itemViewModel.Title,
            Description = itemViewModel.Description, LastEditDate = DateTime.UtcNow.AddHours(3).ToString("MM/dd/yyyy H:mm"),
            Date = itemViewModel.Date, Brand = itemViewModel.Brand, FileName = resultingStrings,
        };

        var currentCollection = _db.Collections.FirstOrDefaultAsync(c => c.CollectionId == newItem.CollectionId).Result;

        currentCollection!.CollectionItems!.Append(newItem);

        await _db.Items.AddAsync(newItem);

        await _db.SaveChangesAsync();

        return await Task.Run(() => RedirectToAction("ViewCollection", "Home", currentCollection));
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
}