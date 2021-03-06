using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ICollections.Models;
using ICollections.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ICollections.Controllers;

public class HomeController : Controller
{
    private readonly IUserService _userService;
    private readonly IUserValidation _userValidation;
    private readonly ICollectionService _collectionService;
    private readonly IItemService _itemService;
    private readonly ILikeService _likeService;
    private readonly ICollectionValidation _collectionValidation;
    private readonly IItemValidation _itemValidation;
    private readonly ILikeValidation _likeValidation;
    
    public HomeController(IUserValidation userValidation, ICollectionValidation collectionValidation, IItemValidation itemValidation, ILikeValidation likeValidation, IUserService userService, ICollectionService collectionService, IItemService itemService, ILikeService likeService)
    {
        _userValidation = userValidation;
        _collectionValidation = collectionValidation;
        _itemValidation = itemValidation;
        _likeValidation = likeValidation;
        _userService = userService;
        _collectionService = collectionService;
        _itemService = itemService;
        _likeService = likeService;
    }
    
    public async Task<IActionResult> Index()
    {
        if (_userValidation.IsUserIsAuthenticatedAndNull(User.Identity!.Name!, User.Identity.IsAuthenticated))
            return await Task.Run(() => RedirectToAction("Logout", "Account"));

        var user = await _userService.GetUserByEmail(User.Identity.Name!);
        
        return await Task.Run(() => View(user));
    }
    
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        if (_userValidation.IsUserNullOrBlocked(User.Identity!.Name!))
            return await Task.Run(() => RedirectToAction("Register", "Account"));

        var user = await _userService.GetUserByEmail(User.Identity.Name!);

        return await Task.Run(() => View(user));
    }

    [Route("/Home/ViewCollection/{collectionId:int}")]
    public async Task<IActionResult> ViewCollection(int collectionId)
    {
        if (_collectionValidation.IsCollectionNull(collectionId)) 
            return await Task.Run(() => RedirectToAction("Profile", "Home"));
        
        var userCollection = _collectionService.GetCollectionById(collectionId);
        userCollection.CollectionItems = _itemService.GetItemsByCollectionId(collectionId);

        return await Task.Run(() => View(userCollection));
    }

    [HttpGet]
    [Route("/Home/ViewItem/{collectionId}/{itemId:int}")]
    public async Task<IActionResult> ViewItem(int itemId)
    {
        if (_itemValidation.IsItemNull(itemId)) 
            return await Task.Run(() => RedirectToAction("Profile", "Home"));

        var item = _itemService.GetItemById(itemId);

        item.Likes = _likeService.GetLikesByItemId(item.Id);

        Console.WriteLine(item.TagsCollection!.Contains("test"));
        

        return await Task.Run(() => View(item));
    }
    
    [HttpPost]
    [Route("/Home/{collectionId::int}/{itemId::int}/ToggleLike")]
    public async Task<IActionResult> ToggleLike(int collectionId, int itemId)
    {
        if (!User.Identity!.IsAuthenticated)
            return await Task.Run(() => BadRequest("You must to be authorized to place likes!"));       
                
        if (_likeValidation.IsUserOwner(User.Identity!.Name!, collectionId)) 
            return await Task.Run(() => BadRequest("You can't like your own item"));

        var item = _itemService.GetItemById(itemId);

        var userId = _userService.GetUserByEmail(User.Identity.Name!).GetAwaiter().GetResult().Id;
        var existingLike = _likeService.IsLikeExists(userId, item.Id);
        
        if (existingLike == null)
        {
            _likeService.AddLike(new Like { UserId = userId, ItemId = itemId });
        }
        else
        {
            _likeService.DeleteLike(existingLike);
        }

        return await Task.Run(NoContent);
    }
    
    [Route("/Home/GetAmountOfLikes/{itemId:int}")]
    public int GetAmountOfLikes(int itemId)
    {
        return _likeService.GetLikesByItemId(itemId).Count;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}