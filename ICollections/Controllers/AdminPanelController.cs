using ICollections.Data.Interfaces;
using ICollections.Models;
using ICollections.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace ICollections.Controllers;

public class AdminPanelController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly ICollectionRepository _collectionRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IUserValidation _userValidation;
    private readonly IDeleteBlob _deleteBlob;

    public AdminPanelController(UserManager<User> userManager, IUserRepository userRepository, IItemRepository itemRepository, ICollectionRepository collectionRepository, IUserValidation userValidation, IDeleteBlob deleteBlob)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _itemRepository = itemRepository;
        _collectionRepository = collectionRepository;
        _userValidation = userValidation;
        _deleteBlob = deleteBlob;
    }
    
    [Authorize]
    public async Task<IActionResult> AdminPanel()
    {
        if (_userValidation.IsUserNull(User.Identity!.Name!))
        {
            return await Task.Run(() => RedirectToAction("Logout", "Account"));   
        }

        if(!_userValidation.IsUserAdminOrSuperAdmin(User.Identity!.Name!))
            return await Task.Run(() => RedirectToAction("Index", "Home"));

        return await Task.Run(() => View(_userRepository.GetAll()));
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
                var itemsToDelete = _itemRepository.FindBy(i => i.CollectionId == collection.Id);
                foreach (var item in itemsToDelete)
                {
                    if (item.FileName != "")
                    {
                        _deleteBlob.DeleteBlob(item.FileName);
                    }
                    
                    _itemRepository.Delete(item);
                }
                
                if (collection.FileName != "")
                {
                    _deleteBlob.DeleteBlob(collection.FileName);
                }
                
                _collectionRepository.Delete(collection);
            }

            await _userManager.DeleteAsync(objectToDelete!);
            
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

            if(objectToBlock.Email == User.Identity!.Name)
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

            if(objectToDemote.Email == User.Identity!.Name && objectToDemote.Role == "user")
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