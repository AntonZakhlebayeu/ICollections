using System.Diagnostics;
using ICollections.Data;
using Microsoft.AspNetCore.Mvc;
using ICollections.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

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
    public IActionResult Index()
    {
        var user = _db.Users.FirstOrDefault(user => user.UserName == User.Identity!.Name);
        
        return View(user);
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}