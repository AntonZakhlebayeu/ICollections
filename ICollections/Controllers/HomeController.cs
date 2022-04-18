using System.Diagnostics;
using ICollections.Data;
using Microsoft.AspNetCore.Mvc;
using ICollections.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ICollections.Controllers;

public class HomeController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _db;

    public HomeController(UserManager<User> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _db = context;
    }
    
    public async Task<ActionResult> Index()
    {
        var user = _db.Users.FirstOrDefaultAsync(user => user.UserName == User.Identity!.Name && user.Status != "Blocked User").Result;

        if (User.Identity!.IsAuthenticated && user == null)
            return await Task.Run(() => RedirectToAction("Logout", "Account"));

        return await Task.Run(() => View(user));
    }
    
    [Authorize]
    public async Task<ActionResult> Profile()
    {
        var user = _db.Users.FirstOrDefaultAsync(user => user.UserName == User.Identity!.Name && user.Status != "Blocked User").Result;
        
        if (user == null)
            return await Task.Run(() => RedirectToAction("Register", "Account"));

        return await Task.Run(() => View(user));
    }
    
    [Authorize]
    public async Task<ActionResult> AdminPanel()
    {
        var user = _db.Users.FirstOrDefaultAsync(u => u.Email == User.Identity!.Name).Result;

        if (user == null)
            return await Task.Run(() => RedirectToAction("Register", "Account"));

        if (user!.Role == "admin")
            return await Task.Run(() => View(_db.Users));

        return await Task.Run(() => RedirectToAction("Index", "Home"));
    }
    
    [Authorize]
    public async Task<IActionResult> Delete(string[] Ids)
    {
        foreach (var id in Ids)
        {
            var objectToDelete = _db.Users.FindAsync(id).Result;

            var result = await _userManager.DeleteAsync(objectToDelete!);
        }

        return await Task.Run(() => RedirectToAction("AdminPanel"));
    }

    [Authorize]
    public async Task<IActionResult> Block(string[] Ids)
    {
        foreach (var id in Ids)
        {
            var objectToBlock = _db.Users.FindAsync(id).Result;
            _db.Users.FindAsync(id)!.Result!.Status = "Blocked User";
            
            await _db.SaveChangesAsync();

            if(objectToBlock!.Email == User.Identity!.Name)
                return await Task.Run(() => RedirectToAction("Logout", "Account"));
        }

        return await Task.Run(() => RedirectToAction("AdminPanel"));
    }

    [Authorize]
    public async Task<IActionResult> Unblock(string[] Ids)
    {
        foreach (var id in Ids)
        {
            var objectToUnBlock = _db.Users.FindAsync(id).Result;
            _db.Users.FindAsync(id)!.Result!.Status = "Active User";
        }

        await _db.SaveChangesAsync();

        return await Task.Run(() => RedirectToAction("AdminPanel"));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}