using ICollections.Models;
using Microsoft.AspNetCore.Identity;

namespace ICollections.Services.Interfaces;

public interface IRoleService
{
    IdentityRole? GetRoleByName(string name);
    Task Promote(User user);
    Task Demote(User user);
    Task AddUserToRole(User user, string roleName);
    Task DeleteUserFromRole(User user);
    Task<bool> IsRoleSuperAdmin(User user);
    Task<bool> IsRoleAdmin(User user);
    Task<bool> IsRoleUser(User user);
    string GetUserRole(User user);
}