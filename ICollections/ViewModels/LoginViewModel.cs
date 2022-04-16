using System.ComponentModel.DataAnnotations;

namespace ICollections.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Not specified Email")]
    public string? Email { get; init; }
         
    [Required(ErrorMessage = "Not specified password")]
    [DataType(DataType.Password)]
    public string? Password { get; init; }

    [Display(Name = "RememberMe?")]
    public bool RememberMe { get; init; }
}