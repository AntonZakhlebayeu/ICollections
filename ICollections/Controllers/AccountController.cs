using ICollections.Models;
using ICollections.Services.Interfaces;
using ICollections.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ICollections.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<User> _signInManager;

    private readonly IUserService _userService;
    private readonly IUserValidation _userValidation;

    public AccountController(SignInManager<User> signInManager, IUserValidation userValidation, IUserService userService)
    {
        _signInManager = signInManager;
        _userValidation = userValidation;
        _userService = userService;
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) 
            return await Task.Run(() => View(model));
        
        var user = new User { Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, NickName = model.NickName, Age = model.Age, UserName = model.Email, RegisterDate = DateTime.Now, LastLoginDate = DateTime.Now, Role = model.Role, Status = "Active User"};
        
        var result = await _userService.SaveNewUser(user, model.Password!);
        
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }
        else
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        
        return await Task.Run(() => View(model));
    }
    
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return await Task.Run(() => View(model));

        if (_userValidation.IsUserNullOrBlocked(model.Email!)) 
            return RedirectToAction("Index", "Home");

        var result = 
            await _signInManager.PasswordSignInAsync(model.Email, model.Password!, model.RememberMe, false);

        if (result.Succeeded)
        {
            var user = await _userService.GetUserByEmail(model.Email!);
            
            user.LastLoginDate = DateTime.Now;

            await _userService.Save();
            
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "Incorrect email and (or) password!");

        return await Task.Run(() => View(model));
    }
    
    
    [Route("/Account/Logout")]
    public async Task<IActionResult> Logout()
    {
        if (_userValidation.IsUserNull(User.Identity!.Name!))
        {
            await _signInManager.SignOutAsync();
            return await Task.Run(() => RedirectToAction("Index", "Home"));
        }
        
        var user = await _userService.GetUserByEmail(User.Identity.Name!);
        user.LastLoginDate = DateTime.Now;

        await _userService.Save();
        await _signInManager.SignOutAsync();
        
        return await Task.Run(() => RedirectToAction("Index", "Home"));
    }
}