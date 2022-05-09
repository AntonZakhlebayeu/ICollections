using Azure.Storage.Blobs;
using ICollections.Data;
using ICollections.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ICollections.Models;
using ICollections.Services;
using ICollections.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ICollections.Controllers;

public class CreateController : Controller
{
    private readonly ISaveFileAsync _saveFileAsync;
    private readonly ICollectionRepository _collectionRepository;
    private readonly IItemRepository _itemRepository;

    public CreateController(ISaveFileAsync saveFileAsync, ICollectionRepository collectionRepository, IItemRepository itemRepository)
    {
        _saveFileAsync = saveFileAsync;
        _collectionRepository = collectionRepository;
        _itemRepository = itemRepository;
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
        if (!ModelState.IsValid || collectionViewModel.Theme == "null") return await Task.Run(() => View(collectionViewModel));

        var resultingString = "";

        if (Request.Form.Files.Count != 0)
        {
            var file = Request.Form.Files.First();
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

        await _collectionRepository.AddAsync(newCollection);

        await _collectionRepository.CommitAsync();

        return await Task.Run(() => RedirectToAction("ViewCollection", "Home", newCollection));
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
        
        if (!ModelState.IsValid) return await Task.Run(() => View(itemViewModel));

        var resultingStrings = ""; 

        if (Request.Form.Files.Count != 0)
        {
            var file = Request.Form.Files.First();
            resultingStrings = _saveFileAsync.SaveFileAsync(file).Result;
        }

        
        var newItem = new Item
        {
            CollectionId = itemViewModel.CollectionId, Title = itemViewModel.Title,
            Description = itemViewModel.Description, LastEditDate = DateTime.UtcNow.AddHours(3).ToString("MM/dd/yyyy H:mm"),
            Date = itemViewModel.Date, Brand = itemViewModel.Brand, FileName = resultingStrings,
        };

        var currentCollection = _collectionRepository.GetSingleAsync(c => c.Id == newItem.CollectionId).Result;

        currentCollection!.CollectionItems!.Add(newItem);

        await _itemRepository.AddAsync(newItem);
        
        await _itemRepository.CommitAsync();

        return await Task.Run(() => RedirectToAction("ViewCollection", "Home", currentCollection));
    }   
}