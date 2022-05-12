using ICollections.Models;
using Microsoft.AspNetCore.Identity;

namespace ICollections.Services.Interfaces;

public interface IUserDatabase
{
    Task<IdentityResult> SaveNewUser(User user, string password);
    Task<User> GetUserByEmail(string email);
    ValueTask Save();
    List<User> GetAllUsers();
    Task<User?> GetUserById(string id);
}