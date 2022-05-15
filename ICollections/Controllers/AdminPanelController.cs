using ICollections.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ICollections.Controllers;

public class AdminPanelController : Controller
{
    private readonly IUserValidation _userValidation;
    private readonly IDeleteBlob _deleteBlob;
    private readonly IUserService _userService;
    private readonly ICollectionService _collectionService;
    private readonly IItemService _itemService;
    private readonly IRoleService _roleService;

    public AdminPanelController(IUserValidation userValidation, IDeleteBlob deleteBlob, IUserService userService, ICollectionService collectionService, IItemService itemService, IRoleService roleService)
    {
        _userService = userService;
        _userValidation = userValidation;
        _deleteBlob = deleteBlob;
        _collectionService = collectionService;
        _itemService = itemService;
        _roleService = roleService;
    }
    
    [Authorize(Roles = "admin, super admin")]
    public async Task<IActionResult> AdminPanel()
    {
        if (_userValidation.IsUserNull(User.Identity!.Name!))
        {
            return await Task.Run(() => RedirectToAction("Logout", "Account"));   
        }

        return await Task.Run(() => View(_userService.GetAllUsers()));
    }
    
    [Authorize(Roles = "admin, super admin")]
    public async Task<IActionResult> Delete(string[] ids)
    {
        foreach (var id in ids)
        {
            var objectToDelete = await _userService.GetUserById(id);

            var userCollections = _collectionService.GetCollectionsByUserId(objectToDelete!.Id);

            foreach (var collection in userCollections)
            {
                var itemsToDelete = _itemService.GetItemsByCollectionId(collection.Id);
                foreach (var item in itemsToDelete)
                {
                    if (item.FileName != "")
                    {
                        _deleteBlob.DeleteBlob(item.FileName);
                    }
                    
                    _itemService.DeleteItem(item);
                }
                
                if (collection.FileName != "")
                {
                    _deleteBlob.DeleteBlob(collection.FileName);
                }
                
                _collectionService.DeleteCollection(collection);
            }

            await _roleService.DeleteUserFromRole(objectToDelete);
            await _userService.Delete(objectToDelete);
        }

        return await Task.Run(() => RedirectToAction("AdminPanel"));
    }

    [Authorize(Roles = "admin, super admin")]
    public async Task<IActionResult> Block(string[] ids)
    {
        foreach (var id in ids)
        {
            var objectToBlock = await _userService.GetUserById(id);
            objectToBlock!.Status = "Blocked User";
            
            await _userService.Save();

            if(objectToBlock.Email == User.Identity!.Name)
                return await Task.Run(() => RedirectToAction("Logout", "Account"));
        }

        return await Task.Run(() => RedirectToAction("AdminPanel"));
    }
    
    [Authorize(Roles = "admin, super admin")]
    public async Task<IActionResult> Promote(string[] ids)
    {
        foreach (var id in ids)
        {
            var objectToPromote = await _userService.GetUserById(id);
            await _roleService.Promote(objectToPromote!);

            await _userService.Save();
        }

        return await Task.Run(() => RedirectToAction("AdminPanel"));
    }
    
    [Authorize(Roles = "admin, super admin")]
    public async Task<IActionResult> Demote(string[] ids)
    {
        foreach (var id in ids)
        {
            var objectToDemote = await _userService.GetUserById(id);
            await _roleService.Demote(objectToDemote!);

            await _userService.Save();
        }

        return await Task.Run(() => RedirectToAction("AdminPanel"));
    }

    [Authorize(Roles = "admin, super admin")]
    public async Task<IActionResult> Unblock(string[] ids)
    {
        foreach (var id in ids)
        {
            var objectToUnBlock = await _userService.GetUserById(id);
            objectToUnBlock!.Status = "Active User";
        }

        await _userService.Save();

        return await Task.Run(() => RedirectToAction("AdminPanel"));
    }
}