﻿using ICollections.Data;
using ICollections.Data.Interfaces;
using ICollections.Models;
using ICollections.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ICollections.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IUserRepository _userRepository;
    
    public AccountController(SignInManager<User> signInManager, IUserRepository userRepository, UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _userRepository = userRepository;
        _userManager = userManager;
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return await Task.Run(() => View(model));

        var user = new User { Email = model.Email, Password = model.Password, FirstName = model.FirstName, LastName = model.LastName, NickName = model.NickName, Age = model.Age, UserName = model.Email, RegisterDate = DateTime.Now, LastLoginDate = DateTime.Now, Role = model.Role, Status = "Active User"};

        var result = await _userManager.CreateAsync(user, model.Password);
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

        var user = _userRepository.GetSingleAsync(u => u.UserName == model.Email).Result;

        if (user == null || user.Status == "Blocked User")
            return RedirectToAction("Index", "Home");

        var result = 
            await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

        if (result.Succeeded)
        {
            user!.LastLoginDate = DateTime.Now;
            await _userRepository.CommitAsync();
            
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ModelState.AddModelError("", "Incorrect email and (or) password!");
        }

        return await Task.Run(() => View(model));
    }
    
    
    [Route("/Account/Logout")]
    public async Task<IActionResult> Logout()
    {
        var user = _userRepository.GetSingleAsync(u => u.UserName == User.Identity!.Name).Result;

        if (user != null)
        {
            user!.LastLoginDate = DateTime.Now;
            await _userRepository.CommitAsync();
        }

        await _signInManager.SignOutAsync();
        return await Task.Run(() => RedirectToAction("Index", "Home"));
    }
}