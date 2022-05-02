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
        
        var newCollection = new Collection
        {
            AuthorId = collectionViewModel.AuthorId, Title = collectionViewModel.Title,
            Description = collectionViewModel.Description, Theme = collectionViewModel.Theme,
            AddDates = collectionViewModel.IncludeDate, AddBrands = collectionViewModel.IncludeBrand,
            AddComments = collectionViewModel.IncludeComments, LastEditDate = DateTime.UtcNow.AddHours(3).ToString("MM/dd/yyyy H:mm"),
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
}