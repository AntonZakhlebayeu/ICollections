using ICollections.Data;
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
        private readonly ApplicationDbContext _db;
 
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = dbContext;
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new User { Email = model.Email, Password = model.Password, FirstName = model.FirstName, LastName = model.LastName, NickName = model.NickName, Age = model.Age, UserName = model.Email, RegisterDate = DateTime.Now, LastLoginDate = DateTime.Now, Role = new IdentityRole("user")};

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
            return View(model);
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
            if (!ModelState.IsValid) return View(model);

            var result = 
                await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                var user = _db.Users.FirstOrDefault(u => u.UserName == model.Email);
                user!.LastLoginDate = DateTime.Now;
                await _db.SaveChangesAsync();
                
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Incorrect email and (or) password!");
            }

            return View(model);
        }

        [HttpPost]
        [Route("/Account/Logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var user = _db.Users.FirstOrDefault(u => u.UserName == User.Identity!.Name);
            user!.LastLoginDate = DateTime.Now;
            await _db.SaveChangesAsync();
            
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }