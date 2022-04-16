using Microsoft.AspNetCore.Identity;

namespace ICollections.Models;

public sealed class User : IdentityUser
{
    public string? FirstName { get; init; }
    public string? LastName { get; set; }
    public string? NickName { get; set; }
    public string? Password { get; set; }
    public int Age { get; set; }
    public DateTime RegisterDate { get; set; }
    public DateTime LastLoginDate { get; set; }
    public IdentityRole? Role { get; set; }
}