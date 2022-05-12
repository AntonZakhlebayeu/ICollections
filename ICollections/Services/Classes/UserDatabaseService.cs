using ICollections.Data.Interfaces;
using ICollections.Models;
using ICollections.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ICollections.Services.Classes;

public class UserDatabaseService : IUserDatabase
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<User> _userManager;

    public UserDatabaseService(IUserRepository userRepository, UserManager<User> userManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
    }

    public async Task<IdentityResult> SaveNewUser(User user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        _userManager.PasswordHasher = new PasswordHasher<User>();
        _userManager.PasswordHasher.HashPassword(user, password);

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
}