using ICollections.Data;
using Microsoft.AspNetCore.Mvc;
using ICollections.Services;
using ICollections.Services.Interfaces;
using ICollections.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace ICollections.Controllers;

public class EditController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly ISaveFileAsync _saveFileAsync;
    private readonly IDeleteBlob _deleteBlob;

    public EditController(ApplicationDbContext context, ISaveFileAsync saveFileAsync, IDeleteBlob deleteBlob)
    {
        _db = context;
        _saveFileAsync = saveFileAsync;
        _deleteBlob = deleteBlob;
    }
    
    
    [Authorize]
    [HttpGet]
    [Route("/Home/EditCollection/{collectionId:int}")]
    public IActionResult EditView(int collectionId)
    {
        ViewBag.collectionId = collectionId;
        
        return View("EditCollection");
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
            resultingString = _saveFileAsync.SaveFileAsync(file).Result;
        }

        editingCollection!.Title = editCollectionViewModel.Title;
        editingCollection!.Description = editCollectionViewModel.Description;
        
        if (Request.Form.Files.Count != 0 && editingCollection!.FileName != "" || 
            Request.Form.Files.Count == 0 && editingCollection!.FileName != "" && editCollectionViewModel.DeleteImage == "true")
        {
            _deleteBlob.DeleteBlob(editingCollection!.FileName);
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
    public IActionResult EditItem(int collectionId, int itemId)
    {
        ViewBag.collectionId = collectionId;
        ViewBag.itemId = itemId;

        return View("EditItem");
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
            resultingString = _saveFileAsync.SaveFileAsync(file).Result;
        }

        editingItem!.Title = editItemViewModel.Title;
        editingItem!.Description = editItemViewModel.Description;
        editingItem!.Date = editItemViewModel.Date;
        editingItem!.Brand = editItemViewModel.Brand;

        if (Request.Form.Files.Count != 0 && editingItem!.FileName != "" || 
            Request.Form.Files.Count == 0 && editingItem!.FileName != "" && editItemViewModel.DeleteImage == "true")
        {
            _deleteBlob.DeleteBlob(editingItem.FileName);
            editingItem!.FileName = resultingString;
        }
        else if (Request.Form.Files.Count != 0 && editingItem!.FileName == "")
        {
            editingItem!.FileName = resultingString;
        }

        await _db.SaveChangesAsync();

        return await Task.Run(() => Redirect($"/Home/ViewItem/{collectionId}/{itemId}"));
    }
}