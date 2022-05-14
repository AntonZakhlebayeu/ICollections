using ICollections.Data.Interfaces;
using ICollections.Models;
using ICollections.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ICollections.Services.Classes;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<User> _userManager;
    private readonly IRoleService _roleService;

    public UserService(IUserRepository userRepository, UserManager<User> userManager, IRoleService roleService)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _roleService = roleService;
    }

    public async Task<IdentityResult> SaveNewUser(User user, string password, string role)
    {
        var result = await _userManager.CreateAsync(user, password);
        _userManager.PasswordHasher = new PasswordHasher<User>();
        _userManager.PasswordHasher.HashPassword(user, password);
        
        await _roleService.AddUserToRole(user, role);

        return result;
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await Task.FromResult(_userRepository.GetSingleAsync(u => u.UserName == email).GetAwaiter().GetResult()!);
    }

    public List<User> GetAllUsers()
    {
        return _userRepository.GetAll().ToList();
    }

    public async ValueTask Save()
    {
        await _userRepository.CommitAsync();
    }

    public async Task<User?> GetUserById(string id)
    {
        return await _userRepository.FindAsync(id);
    }

    public async Task Delete(User user)
    {
        _userRepository.Delete(user);
        await _userRepository.CommitAsync();
    }
}