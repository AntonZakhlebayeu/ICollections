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
    private readonly IUserValidation _userValidation;
    private readonly IDeleteBlob _deleteBlob;
    private readonly IUserDatabase _userDatabase;
    private readonly ICollectionDatabase _collectionDatabase;
    private readonly IItemDatabase _itemDatabase;

    public AdminPanelController(UserManager<User> userManager, IUserValidation userValidation, IDeleteBlob deleteBlob, IUserDatabase userDatabase, ICollectionDatabase collectionDatabase, IItemDatabase itemDatabase)
    {
        _userManager = userManager;
        _userValidation = userValidation;
        _deleteBlob = deleteBlob;
        _userDatabase = userDatabase;
        _collectionDatabase = collectionDatabase;
        _itemDatabase = itemDatabase;
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

        return await Task.Run(() => View(_userDatabase.GetAllUsers()));
    }
    
    [Authorize]
    public async Task<IActionResult> Delete(string[] ids)
    {
        foreach (var id in ids)
        {
            var objectToDelete = await _userDatabase.GetUserById(id);

            var userCollections = _collectionDatabase.GetCollectionsByUserId(objectToDelete!.Id);

            foreach (var collection in userCollections)
            {
                var itemsToDelete = _itemDatabase.GetItemsByCollectionId(collection.Id);
                foreach (var item in itemsToDelete)
                {
                    if (item.FileName != "")
                    {
                        _deleteBlob.DeleteBlob(item.FileName);
                    }
                    
                    _itemDatabase.DeleteItem(item);
                }
                
                if (collection.FileName != "")
                {
                    _deleteBlob.DeleteBlob(collection.FileName);
                }
                
                _collectionDatabase.DeleteCollection(collection);
            }

            await _userManager.DeleteAsync(objectToDelete);
        }

        return await Task.Run(() => RedirectToAction("AdminPanel"));
    }

    [Authorize]
    public async Task<IActionResult> Block(string[] ids)
    {
        foreach (var id in ids)
        {
            var objectToBlock = await _userDatabase.GetUserById(id);
            objectToBlock!.Status = "Blocked User";
            
            await _userDatabase.Save();

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
            var objectToPromote = await _userDatabase.GetUserById(id);
            objectToPromote!.Role = "admin";

            await _userDatabase.Save();
        }

        return await Task.Run(() => RedirectToAction("AdminPanel"));
    }
    
    [Authorize]
    public async Task<IActionResult> Demote(string[] ids)
    {
        foreach (var id in ids)
        {
            var objectToDemote = await _userDatabase.GetUserById(id);
            objectToDemote!.Role = "user";

            await _userDatabase.Save();

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
            var objectToUnBlock = await _userDatabase.GetUserById(id);
            objectToUnBlock!.Status = "Active User";
        }

        await _userDatabase.Save();

        return await Task.Run(() => RedirectToAction("AdminPanel"));
    }
}