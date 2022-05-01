using System.Diagnostics;
using ICollections.Data;
using Microsoft.AspNetCore.Mvc;
using ICollections.Models;
using ICollections.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ICollections.Controllers;

public class HomeController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _db;

    public HomeController(UserManager<User> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _db = context;
    }
    
    public async Task<IActionResult> Index()
    {
        var user = _db.Users.FirstOrDefaultAsync(user => user.UserName == User.Identity!.Name && user.Status != "Blocked User").Result;

        if (User.Identity!.IsAuthenticated && user == null)
            return await Task.Run(() => RedirectToAction("Logout", "Account"));

        return await Task.Run(() => View(user));
    }
    
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var user = _db.Users.FirstOrDefaultAsync(user => user.UserName == User.Identity!.Name && user.Status != "Blocked User").Result;
        
        if (user == null)
            return await Task.Run(() => RedirectToAction("Register", "Account"));

        return await Task.Run(() => View(user));
    }
    
    [Authorize]
    public async Task<IActionResult> AdminPanel()
    {
        var user = _db.Users.FirstOrDefaultAsync(u => u.Email == User.Identity!.Name).Result;

        if (user == null)
            return await Task.Run(() => RedirectToAction("Register", "Account"));

        if (user!.Role is "admin" or "super admin")
            return await Task.Run(() => View(_db.Users));

        return await Task.Run(() => RedirectToAction("Index", "Home"));
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

       return await Task.Run(() => RedirectToAction("ViewCollection", newCollection));
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

        return await Task.Run(() => RedirectToAction("ViewCollection", currentCollection));
    }
    
    [Route("/Home/ViewCollection/{collectionId:int}")]
    public async Task<IActionResult> ViewCollection(int collectionId)
    {
        var userCollection = _db.Collections.FirstOrDefaultAsync(c => c.CollectionId == collectionId).Result;
        
        if (userCollection == null) return await Task.Run(() => RedirectToAction("Profile", "Home"));

        userCollection!.CollectionItems = _db.Items.Where(i => i.CollectionId == userCollection.CollectionId).ToList();

        return await Task.Run(() => View(userCollection));
    }

    [HttpGet]
    [Route("/Home/ViewItem/{collectionId}/{itemId:int}")]
    public async Task<IActionResult> ViewItem(int itemId)
    {
        var item = _db.Items.FirstOrDefaultAsync(i => i.Id == itemId).Result;

        if (item == null) return await Task.Run(() => RedirectToAction("Profile", "Home"));

        return await Task.Run(() => View(item));
    }
    
    [Route("/Home/ViewItem/{collectionId}/DeleteCollection")]
    public async Task<IActionResult> DeleteCollection(int collectionId)
    {
        var objectToDelete = _db.Collections.FindAsync(collectionId).Result;

        var result = _db.Collections.Remove(objectToDelete!);

        await _db.SaveChangesAsync();

        return await Task.Run(() => RedirectToAction("Profile", "Home"));
    }
    
    [Route("/Home/ViewItem/{collectionId}/{itemId:int}/DeleteItem")]
    public async Task<IActionResult> DeleteItem(int itemId, int collectionId)
    {
        var objectToDelete = _db.Items.FindAsync(itemId).Result;

        var result = _db.Items.Remove(objectToDelete!);

        await _db.SaveChangesAsync();

        return await Task.Run(() => Redirect($"/Home/ViewCollection/{collectionId}"));
    }

    [Authorize]
    public async Task<IActionResult> Delete(string[] Ids)
    {
        foreach (var id in Ids)
        {
            var objectToDelete = _db.Users.FindAsync(id).Result;

            var result = await _userManager.DeleteAsync(objectToDelete!);
        }

        return await Task.Run(() => RedirectToAction("AdminPanel"));
    }

    [Authorize]
    public async Task<IActionResult> Block(string[] Ids)
    {
        foreach (var id in Ids)
        {
            var objectToBlock = _db.Users.FindAsync(id).Result;
            objectToBlock!.Status = "Blocked User";
            
            await _db.SaveChangesAsync();

            if(objectToBlock!.Email == User.Identity!.Name)
                return await Task.Run(() => RedirectToAction("Logout", "Account"));
        }

        return await Task.Run(() => RedirectToAction("AdminPanel"));
    }
    
    [Authorize]
    public async Task<IActionResult> Promote(string[] Ids)
    {
        foreach (var id in Ids)
        {
            var objectToPromote = _db.Users.FindAsync(id).Result;
            objectToPromote!.Role = "admin";
            
            await _db.SaveChangesAsync();
        }

        return await Task.Run(() => RedirectToAction("AdminPanel"));
    }
    
    [Authorize]
    public async Task<IActionResult> Demote(string[] Ids)
    {
        foreach (var id in Ids)
        {
            var objectToDemote = _db.Users.FindAsync(id).Result;
            objectToDemote!.Role = "user";
            
            await _db.SaveChangesAsync();

            if(objectToDemote!.Email == User.Identity!.Name && objectToDemote!.Role == "user")
                return await Task.Run(() => RedirectToAction("Index", "Home"));
        }

        return await Task.Run(() => RedirectToAction("AdminPanel"));
    }

    [Authorize]
    public async Task<IActionResult> Unblock(string[] Ids)
    {
        foreach (var id in Ids)
        {
            var objectToUnBlock = _db.Users.FindAsync(id).Result;
            objectToUnBlock!.Status = "Active User";
        }

        await _db.SaveChangesAsync();

        return await Task.Run(() => RedirectToAction("AdminPanel"));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}