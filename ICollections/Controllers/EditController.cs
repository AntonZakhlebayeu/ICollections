using System.Diagnostics;
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

    public EditController(ApplicationDbContext context)
    {
        _db = context;
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

        editingCollection!.Title = editCollectionViewModel.Title;
        editingCollection!.Description = editCollectionViewModel.Description;

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

        var editedItem = _db.Items.FindAsync(itemId).Result;

        editedItem!.Title = editItemViewModel.Title;
        editedItem!.Description = editItemViewModel.Description;

        await _db.SaveChangesAsync();

        return await Task.Run(() => Redirect($"/Home/ViewItem/{collectionId}/{itemId}"));
    }
}