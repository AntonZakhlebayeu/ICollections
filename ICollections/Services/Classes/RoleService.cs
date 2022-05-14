using ICollections.Data.Interfaces;
using ICollections.Models;
using ICollections.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ICollections.Services.Classes;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;

    public RoleService(IRoleRepository roleRepository, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
    {
        _roleRepository = roleRepository;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public IdentityRole? GetRoleByName(string name)
    {
        return _roleRepository.GetSingle(r => r.Name == name);
    }

    public async Task Promote(User user)
    {
        await _userManager.RemoveFromRoleAsync(user, GetUserRole(user));
        await _userManager.AddToRoleAsync(user, "admin");
    }

    public async Task Demote(User user)
    {
        await _userManager.RemoveFromRoleAsync(user, GetUserRole(user));
        await _userManager.AddToRoleAsync(user, "user");
    }

    public async Task AddUserToRole(User user, string roleName)
    {
        await _userManager.AddToRoleAsync(user, roleName);
    }

    public async Task DeleteUserFromRole(User user)
    {
        await _userManager.RemoveFromRoleAsync(user, GetUserRole(user));
    }

    public async Task<bool> IsRoleSuperAdmin(User user)
    {
        var role = GetRoleByName("super admin");
        return await _userManager.IsInRoleAsync(user, role!.NormalizedName);
    }

    public async Task<bool> IsRoleAdmin(User user)
    {
        var role = GetRoleByName("admin");
        return await _userManager.IsInRoleAsync(user, role!.NormalizedName);
    }

    public async Task<bool> IsRoleUser(User user)
    {
        var role = GetRoleByName("user");
        return await _userManager.IsInRoleAsync(user, role!.NormalizedName);
    }

    public string GetUserRole(User user)
    {
        return (from role in _roleManager.Roles.ToList() where _userManager.IsInRoleAsync(user, role.NormalizedName).GetAwaiter().GetResult() select role.Name).FirstOrDefault()!;
    }
}