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

    [Authorize]
    public async Task<ActionResult> Index()
    {
        var user = _db.Users.FirstOrDefaultAsync(user => user.UserName == User.Identity!.Name);

        return await Task.Run(() => View(user.Result));
    }

    public async Task<ActionResult> Profile()
    {
        var user = _db.Users.FirstOrDefaultAsync(user => user.UserName == User.Identity!.Name);

        User.IsInRole("user");

        return await Task.Run(() => View(user.Result));
    }
    
    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}