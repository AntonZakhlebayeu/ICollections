using Microsoft.AspNetCore.Mvc;
using ICollections.Models;
using ICollections.Services;
using ICollections.Services.Interfaces;
using ICollections.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace ICollections.Controllers;

public class CreateController : Controller
{
    private readonly ISaveFileAsync _saveFileAsync;
    private readonly ICollectionService _collectionService;
    private readonly IItemService _itemService;
    private readonly ITagService _tagService;

    public CreateController(ISaveFileAsync saveFileAsync, IItemService itemService, ICollectionService collectionService, ITagService tagService)
    {
        _saveFileAsync = saveFileAsync;
        _itemService = itemService;
        _collectionService = collectionService;
        _tagService = tagService;
    }
    
    [Authorize]
    [HttpGet]
    public IActionResult CreateView()
    {
        return View("CreateCollection");
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateCollection(CollectionViewModel collectionViewModel)
    {
        if (!ModelState.IsValid || collectionViewModel.Theme == "null") 
            return await Task.Run(() => View(collectionViewModel));

        var resultingString = "";

        if (Request.Form.Files.Count != 0)
        {
            var file = Request.Form.Files[0];
            resultingString = _saveFileAsync.SaveFileAsync(file).Result;
        }

        var newCollection = new Collection
        {
            AuthorId = collectionViewModel.AuthorId, Title = collectionViewModel.Title,
            Description = collectionViewModel.Description, Theme = collectionViewModel.Theme,
            AddDates = collectionViewModel.IncludeDate, AddBrands = collectionViewModel.IncludeBrand,
            AddComments = collectionViewModel.IncludeComments, LastEditDate = DateTime.UtcNow.AddHours(3).ToString("MM/dd/yyyy H:mm"),
            FileName = resultingString,
        };

        _collectionService.AddCollection(newCollection);

        return await Task.Run(() => RedirectToAction("ViewCollection", "Home", _collectionService.GetCollectionById(newCollection.Id)));
    }

    [HttpGet]
    [Route("/Home/AddItem/{collectionId:int}")]
    public IActionResult AddItem(int collectionId)
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
        
        if (!ModelState.IsValid) 
            return await Task.Run(() => View(itemViewModel));

        var resultingStrings = ""; 

        if (Request.Form.Files.Count != 0)
        {
            var file = Request.Form.Files[0];
            resultingStrings = _saveFileAsync.SaveFileAsync(file).Result;
        }

        
        var newItem = new Item
        {
            CollectionId = itemViewModel.CollectionId, Title = itemViewModel.Title,
            Description = itemViewModel.Description, LastEditDate = DateTime.UtcNow.AddHours(3).ToString("MM/dd/yyyy H:mm"),
            Date = itemViewModel.Date, Brand = itemViewModel.Brand, FileName = resultingStrings, TagsCollection = itemViewModel.Tags!.Remove(itemViewModel.Tags.Length - 1)
        };

        var currentCollection = _collectionService.GetCollectionByItemId(newItem.CollectionId);

        currentCollection.CollectionItems!.Add(newItem);
        
        var tags = itemViewModel.Tags!.Remove(itemViewModel.Tags.Length - 1).Split(" ").ToList();

        await _itemService.AddItem(newItem);
        
        foreach (var tag in tags)
        {
            await _tagService.AddTag(tag);
        }

        return await Task.Run(() => RedirectToAction("ViewCollection", "Home", currentCollection));
    }   
}