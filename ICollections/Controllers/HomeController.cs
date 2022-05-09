using System.Diagnostics;
using ICollections.Data;
using ICollections.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ICollections.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ICollections.Controllers;

public class HomeController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly ICollectionRepository _collectionRepository;
    private readonly IItemRepository _itemRepository;
    
    public HomeController(IUserRepository userRepository, ICollectionRepository collectionRepository, IItemRepository itemRepository)
    {
        _userRepository = userRepository;
        _collectionRepository = collectionRepository;
        _itemRepository = itemRepository;
    }
    
    public async Task<IActionResult> Index()
    {
        var user = _userRepository
            .GetSingleAsync(user => user.UserName == User.Identity!.Name && user.Status != "Blocked User").Result;

        if (User.Identity!.IsAuthenticated && user == null)
            return await Task.Run(() => RedirectToAction("Logout", "Account"));

        return await Task.Run(() => View(user));
    }
    
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var user = _userRepository
            .GetSingleAsync(user => user.UserName == User.Identity!.Name && user.Status != "Blocked User").Result;
        
        if (user == null)
            return await Task.Run(() => RedirectToAction("Register", "Account"));

        return await Task.Run(() => View(user));
    }

    [Route("/Home/ViewCollection/{collectionId:int}")]
    public async Task<IActionResult> ViewCollection(int collectionId)
    {
        var userCollection = _collectionRepository.GetSingleAsync(c => c.Id == collectionId).Result;
        
        if (userCollection == null) return await Task.Run(() => RedirectToAction("Profile", "Home"));
        
        userCollection.CollectionItems = _itemRepository.FindBy(i => i.CollectionId == userCollection.Id).ToList();

        return await Task.Run(() => View(userCollection));
    }

    [HttpGet]
    [Route("/Home/ViewItem/{collectionId}/{itemId:int}")]
    public async Task<IActionResult> ViewItem(int itemId)
    {
        var item = _itemRepository.GetSingleAsync(i => i.Id == itemId).Result;

        if (item == null) return await Task.Run(() => RedirectToAction("Profile", "Home"));

        return await Task.Run(() => View(item));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}