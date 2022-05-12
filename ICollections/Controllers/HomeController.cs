using System.Diagnostics;
using ICollections.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ICollections.Models;
using ICollections.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ICollections.Controllers;

public class HomeController : Controller
{
    private readonly IUserDatabase _userDatabase;
    private readonly IUserValidation _userValidation;
    private readonly ICollectionDatabase _collectionDatabase;
    private readonly IItemDatabase _itemDatabase;
    private readonly ICollectionValidation _collectionValidation;
    private readonly IItemValidation _itemValidation;
    private readonly ILikeRepository _likeRepository;
    private readonly ILikeValidation _likeValidation;
    
    public HomeController(IUserValidation userValidation, ICollectionValidation collectionValidation, IItemValidation itemValidation, ILikeRepository likeRepository, ILikeValidation likeValidation, IUserDatabase userDatabase, ICollectionDatabase collectionDatabase, IItemDatabase itemDatabase)
    {
        _userValidation = userValidation;
        _collectionValidation = collectionValidation;
        _itemValidation = itemValidation;
        _likeRepository = likeRepository;
        _likeValidation = likeValidation;
        _userDatabase = userDatabase;
        _collectionDatabase = collectionDatabase;
        _itemDatabase = itemDatabase;
    }
    
    public async Task<IActionResult> Index()
    {
        if (_userValidation.IsUserIsAuthenticatedAndNull(User.Identity!.Name!, User.Identity.IsAuthenticated))
            return await Task.Run(() => RedirectToAction("Logout", "Account"));

        var user = await _userDatabase.GetUserByEmail(User.Identity.Name!);
        
        return await Task.Run(() => View(user));
    }
    
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        if (_userValidation.IsUserNullOrBlocked(User.Identity!.Name!))
            return await Task.Run(() => RedirectToAction("Register", "Account"));

        var user = await _userDatabase.GetUserByEmail(User.Identity.Name!);

        return await Task.Run(() => View(user));
    }

    [Route("/Home/ViewCollection/{collectionId:int}")]
    public async Task<IActionResult> ViewCollection(int collectionId)
    {
        if (_collectionValidation.IsCollectionNull(collectionId)) 
            return await Task.Run(() => RedirectToAction("Profile", "Home"));
        
        var userCollection = _collectionDatabase.GetCollectionById(collectionId);
        userCollection.CollectionItems = _itemDatabase.GetItemsByCollectionId(collectionId);

        return await Task.Run(() => View(userCollection));
    }

    [HttpGet]
    [Route("/Home/ViewItem/{collectionId}/{itemId:int}")]
    public async Task<IActionResult> ViewItem(int itemId)
    {
        if (_itemValidation.IsItemNull(itemId)) 
            return await Task.Run(() => RedirectToAction("Profile", "Home"));

        var item = _itemDatabase.GetItemById(itemId);

        item.Likes = _likeRepository.FindBy(l => l.ItemId == item.Id).ToList();

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

        var item = _itemDatabase.GetItemById(itemId);

        var userId = _userDatabase.GetUserByEmail(User.Identity.Name!).GetAwaiter().GetResult().Id;
        var existingLike = item.Likes.Find(l => l.UserId == userId);
        
        if (existingLike == null)
        {
            _likeRepository.Add(new Like { UserId = userId, ItemId = itemId });
        }
        else 
        {
            _likeRepository.Delete(existingLike);
            
        }
        
        await _likeRepository.CommitAsync();
        
        return await Task.Run(NoContent);
    }
    
    [Route("/Home/GetAmountOfLikes/{itemId:int}")]
    public int GetAmountOfLikes(int itemId)
    {
        return _likeRepository.FindBy(l => l.ItemId == itemId).Count();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}