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

        if (user!.Role == "admin" && user!.Status != "Blocked User")
            return await Task.Run(() => View(_db.Users));

        return await Task.Run(() => RedirectToAction("Index", "Home"));
    }
    
    public IActionResult Delete(int[] Ids)
    {

        if (IsUserInvalid(User.Identity?.Name))
            return RedirectToAction("Login", "Account");

        foreach (var id in Ids)
        {
            var objectToDelete = _db.Users.Find(id);
            _db.Users.Remove(objectToDelete);
        }

        _db.SaveChanges();

        return Redirect("~/");

    }

    public bool IsUserInvalid(string? Email)
    {
        var user = _db.Users.First(u => u.Email == Email);

        return user.Status != "Active User";
    }

    public IActionResult Block(int[] Ids)
    {

        if (IsUserInvalid(User.Identity?.Name))
            return RedirectToAction("Login", "Account");

        foreach (var id in Ids)
        {
            var objectToDelete = _db.Users.Find(id);
            _db.Users.Find(id)!.Status = "Blocked User";
        }

        _db.SaveChanges();

        return Redirect("~/");

    }

    public IActionResult Unblock(int[] Ids)
    {

        if (IsUserInvalid(User.Identity?.Name))
            return RedirectToAction("Login", "Account");

        foreach (var id in Ids)
        {
            var objectToDelete = _db.Users.Find(id);
            _db.Users.Find(id)!.Status = "Active User";
        }

        _db.SaveChanges();

        return Redirect("~/");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}