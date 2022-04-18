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
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _db;

    public HomeController(ILogger<HomeController> logger, UserManager<User> userManager, ApplicationDbContext context)
    {
        _logger = logger;
        _userManager = userManager;
        _db = context;
    }
    
    public async Task<ActionResult> Index()
    {
        var user = _db.Users.FirstOrDefaultAsync(user => user.UserName == User.Identity!.Name);

        return await Task.Run(() => View(user.Result));
    }
    
    [Authorize]
    public async Task<ActionResult> Profile()
    {
        var user = _db.Users.FirstOrDefaultAsync(user => user.UserName == User.Identity!.Name);

        return await Task.Run(() => View(user.Result));
    }
    
    [Authorize]
    public async Task<ActionResult> AdminPanel()
    {
        var user = _db.Users.FirstOrDefaultAsync(u => u.Email == User.Identity!.Name).Result;

        if (user == null)
        {
            //TODO
            //Set null authenticate

            return await Task.Run(() => RedirectToAction("Register", "Account"));
        }

        if (user!.Role == "admin" && user!.Status != "Blocked User")
            return await Task.Run(() => View(_db.Users));

        return await Task.Run(() => RedirectToAction("Index", "Home"));
    }
    
    [Authorize]
    public async Task<IActionResult> Delete(string[] Ids)
    {
        foreach (var id in Ids)
        {
            var objectToDelete = _db.Users.FindAsync(id).Result;
            _db.Users.Remove(objectToDelete!);
        }

        await _db.SaveChangesAsync();

        return await Task.Run(() => Redirect("/Home/AdminPanel"));
    }

    [Authorize]
    public async Task<IActionResult> Block(string[] Ids)
    {
        foreach (var id in Ids)
        {
            var objectToDelete = _db.Users.FindAsync(id).Result;
            _db.Users.FindAsync(id)!.Result.Status = "Blocked User";
        }

        _db.SaveChangesAsync();

        return await Task.Run(() => Redirect("/Home/AdminPanel"));
    }

    [Authorize]
    public async Task<IActionResult> Unblock(string[] Ids)
    {
        foreach (var id in Ids)
        {
            var objectToDelete = _db.Users.FindAsync(id).Result;
            _db.Users.FindAsync(id)!.Result.Status = "Active User";
        }

        _db.SaveChangesAsync();

        return await Task.Run(() => Redirect("/Home/AdminPanel"));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}