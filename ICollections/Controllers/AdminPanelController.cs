using ICollections.Data;
using ICollections.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace ICollections.Controllers;

public class AdminPanelController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _db;

    public AdminPanelController(UserManager<User> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _db = context;
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
    public async Task<IActionResult> Delete(string[] ids)
    {
        foreach (var id in ids)
        {
            var objectToDelete = _db.Users.FindAsync(id).Result;

            var userCollections = _db.Collections.Where(i => i.AuthorId == objectToDelete!.Id);

            foreach (var collection in userCollections)
            {
                var itemsToDelete = _db.Items.Where(i => i.CollectionId == collection!.CollectionId);
                foreach (var item in itemsToDelete)
                {
                    _db.Items.Remove(item);
                }
            }

            var result = await _userManager.DeleteAsync(objectToDelete!);
        }

        return await Task.Run(() => RedirectToAction("AdminPanel"));
    }

    [Authorize]
    public async Task<IActionResult> Block(string[] ids)
    {
        foreach (var id in ids)
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
    public async Task<IActionResult> Promote(string[] ids)
    {
        foreach (var id in ids)
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
    public async Task<IActionResult> Unblock(string[] ids)
    {
        foreach (var id in ids)
        {
            var objectToUnBlock = _db.Users.FindAsync(id).Result;
            objectToUnBlock!.Status = "Active User";
        }

        await _db.SaveChangesAsync();

        return await Task.Run(() => RedirectToAction("AdminPanel"));
    }
}