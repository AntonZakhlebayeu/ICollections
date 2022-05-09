using ICollections.Data;
using ICollections.Data.Interfaces;
using ICollections.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace ICollections.Controllers;

public class AdminPanelController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly ICollectionRepository _collectionRepository;
    private readonly IItemRepository _itemRepository;

    public AdminPanelController(UserManager<User> userManager, IUserRepository userRepository, IItemRepository itemRepository, ICollectionRepository collectionRepository)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _itemRepository = itemRepository;
        _collectionRepository = collectionRepository;
    }
    
    [Authorize]
    public async Task<IActionResult> AdminPanel()
    {
        var user = _userRepository.GetSingleAsync(u => u.Email == User.Identity!.Name).Result;

        if (user == null)
            return await Task.Run(() => RedirectToAction("Register", "Account"));

        if (user!.Role is "admin" or "super admin")
            return await Task.Run(() => View(_userRepository.GetAll()));

        return await Task.Run(() => RedirectToAction("Index", "Home"));
    }
    
    [Authorize]
    public async Task<IActionResult> Delete(string[] ids)
    {
        foreach (var id in ids)
        {
            var objectToDelete = _userRepository.FindAsync(id).Result;

            var userCollections = _collectionRepository.FindBy(i => i.AuthorId == objectToDelete!.Id);

            foreach (var collection in userCollections)
            {
                var itemsToDelete = _itemRepository.FindBy(i => i.CollectionId == collection!.Id);
                foreach (var item in itemsToDelete)
                {
                    _itemRepository.Delete(item);
                }
                _collectionRepository.Delete(collection);
            }

            var result = await _userManager.DeleteAsync(objectToDelete!);
            
            await _collectionRepository.CommitAsync();
            await _itemRepository.CommitAsync();
        }

        return await Task.Run(() => RedirectToAction("AdminPanel"));
    }

    [Authorize]
    public async Task<IActionResult> Block(string[] ids)
    {
        foreach (var id in ids)
        {
            var objectToBlock = _userRepository.FindAsync(id).Result;
            objectToBlock!.Status = "Blocked User";
            
            await _userRepository.CommitAsync();

            if(objectToBlock!.Email == User.Identity!.Name)
                return await Task.Run(() => RedirectToAction("Logout", "Account"));
        }

        return await Task.Run(() => RedirectToAction("AdminPanel"));
    }
    
    [Authorize]
    public async Task<IActionResult> Promote(string[] ids)
    {
        foreach (var id in ids)
        {
            var objectToPromote = _userRepository.FindAsync(id).Result;
            objectToPromote!.Role = "admin";
            
            await _userRepository.CommitAsync();
        }

        return await Task.Run(() => RedirectToAction("AdminPanel"));
    }
    
    [Authorize]
    public async Task<IActionResult> Demote(string[] ids)
    {
        foreach (var id in ids)
        {
            var objectToDemote = _userRepository.FindAsync(id).Result;
            objectToDemote!.Role = "user";
            
            await _userRepository.CommitAsync();

            if(objectToDemote!.Email == User.Identity!.Name && objectToDemote!.Role == "user")
                return await Task.Run(() => RedirectToAction("Index", "Home"));
        }

        return await Task.Run(() => RedirectToAction("AdminPanel"));
    }

    [Authorize]
    public async Task<IActionResult> Unblock(string[] ids)
    {
        foreach (var id in ids)
        {
            var objectToUnBlock = _userRepository.FindAsync(id).Result;
            objectToUnBlock!.Status = "Active User";
        }

        await _userRepository.CommitAsync();

        return await Task.Run(() => RedirectToAction("AdminPanel"));
    }
}