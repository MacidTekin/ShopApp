using System.ComponentModel.DataAnnotations;

namespace ShopApp.Web.Models;

public class LoginViewModel
{   
    [Required]
    [EmailAddress]
    [Display(Name ="Eposta")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(10, ErrorMessage = "Max. 10 karakter belirtiniz.", MinimumLength=6)]
    [DataType(DataType.Password)]
    [Display(Name ="Parola")]
    public string Password { get; set; } = string.Empty;
}