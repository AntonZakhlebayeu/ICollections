using System.Diagnostics;
using ICollections.Data;
using ICollections.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ICollections.Models;
using ICollections.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ICollections.Controllers;

public class HomeController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly ICollectionRepository _collectionRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IUserValidation _userValidation;
    private readonly ICollectionValidation _collectionValidation;
    private readonly IItemValidation _itemValidation;
    
    public HomeController(IUserRepository userRepository, ICollectionRepository collectionRepository, IItemRepository itemRepository, IUserValidation userValidation, ICollectionValidation collectionValidation, IItemValidation itemValidation)
    {
        _userRepository = userRepository;
        _collectionRepository = collectionRepository;
        _itemRepository = itemRepository;
        _userValidation = userValidation;
        _collectionValidation = collectionValidation;
        _itemValidation = itemValidation;
    }
    
    public async Task<IActionResult> Index()
    {
        if (_userValidation.IsUserIsAuthenticatedAndNull(User!.Identity!.Name!, User.Identity.IsAuthenticated))
            return await Task.Run(() => RedirectToAction("Logout", "Account"));

        var user = _userRepository.GetSingleAsync(user => user.UserName == User!.Identity!.Name).Result;
        
        return await Task.Run(() => View(user));
    }
    
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        if (_userValidation.IsUserNullOrBlocked(User!.Identity!.Name!))
            return await Task.Run(() => RedirectToAction("Register", "Account"));

        var user = _userRepository.GetSingleAsync(user => user.UserName == User.Identity!.Name).Result;

        return await Task.Run(() => View(user));
    }

    [Route("/Home/ViewCollection/{collectionId:int}")]
    public async Task<IActionResult> ViewCollection(int collectionId)
    {
        if (_collectionValidation.IsCollectionNull(collectionId)) 
            return await Task.Run(() => RedirectToAction("Profile", "Home"));
        
        var userCollection = _collectionRepository.GetSingleAsync(c => c.Id == collectionId).Result;
        userCollection!.CollectionItems = _itemRepository.FindBy(i => i.CollectionId == userCollection.Id).ToList();

        return await Task.Run(() => View(userCollection));
    }

    [HttpGet]
    [Route("/Home/ViewItem/{collectionId}/{itemId:int}")]
    public async Task<IActionResult> ViewItem(int itemId)
    {
        if (_itemValidation.IsItemNull(itemId)) 
            return await Task.Run(() => RedirectToAction("Profile", "Home"));

        var item = _itemRepository.GetSingleAsync(i => i.Id == itemId).Result;

        return await Task.Run(() => View(item));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}