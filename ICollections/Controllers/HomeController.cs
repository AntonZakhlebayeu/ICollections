using System.Diagnostics;
using ICollections.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ICollections.Models;
using ICollections.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ICollections.Controllers;

public class HomeController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly ICollectionRepository _collectionRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IUserValidation _userValidation;
    private readonly ICollectionValidation _collectionValidation;
    private readonly IItemValidation _itemValidation;
    private readonly ILikeRepository _likeRepository;
    private readonly ILikeValidation _likeValidation;
    
    public HomeController(IUserRepository userRepository, ICollectionRepository collectionRepository, IItemRepository itemRepository, IUserValidation userValidation, ICollectionValidation collectionValidation, IItemValidation itemValidation, ILikeRepository likeRepository, ILikeValidation likeValidation)
    {
        _userRepository = userRepository;
        _collectionRepository = collectionRepository;
        _itemRepository = itemRepository;
        _userValidation = userValidation;
        _collectionValidation = collectionValidation;
        _itemValidation = itemValidation;
        _likeRepository = likeRepository;
        _likeValidation = likeValidation;
    }
    
    public async Task<IActionResult> Index()
    {
        if (_userValidation.IsUserIsAuthenticatedAndNull(User.Identity!.Name!, User.Identity.IsAuthenticated))
            return await Task.Run(() => RedirectToAction("Logout", "Account"));

        var user = _userRepository.GetSingleAsync(user => user.UserName == User.Identity!.Name).Result;
        
        return await Task.Run(() => View(user));
    }
    
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        if (_userValidation.IsUserNullOrBlocked(User.Identity!.Name!))
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

        item!.Likes = _likeRepository.FindBy(l => l.ItemId == item.Id).ToList();

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
        
        var item = _itemRepository.GetSingleAsync(s => s!.Id == itemId, s => s.Likes).Result;

        var userId = _userRepository.GetSingleAsync(u => u.UserName == User.Identity.Name).Result!.Id;
        var existingLike = item!.Likes.Find(l => l.UserId == userId);
        
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