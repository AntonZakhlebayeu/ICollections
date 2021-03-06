using ICollections.Models;
using Microsoft.AspNetCore.Identity;

namespace ICollections.Services.Interfaces;

public interface IUserService
{
    Task<IdentityResult> SaveNewUser(User user, string password, string role);
    Task<User> GetUserByEmail(string email);
    ValueTask Save();
    List<User> GetAllUsers();
    Task<User?> GetUserById(string id);
    Task Delete(User user);
}