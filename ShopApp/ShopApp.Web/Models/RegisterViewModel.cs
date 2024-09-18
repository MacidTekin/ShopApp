using System.ComponentModel.DataAnnotations;

namespace ShopApp.Web.Models;

public class RegisterViewModel
{   
    [Required]
    [Display(Name ="Kullanıcı Adı")]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [Display(Name ="Ad Soyad")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [Display(Name ="Eposta")]
    public string Email { get; set; } = string.Empty;
    
    
    [Display(Name ="Profil Resmi")]
    public string Image { get; set; } = string.Empty;

    [Required]
    [StringLength(10, ErrorMessage = "Max. 10 karakter belirtiniz.", MinimumLength=6)]
    [DataType(DataType.Password)]
    [Display(Name ="Parola")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password) ,ErrorMessage = "Parolanız eşleşmiyor.")]
    [Display(Name ="Parola Tekrar")]
    public string ConfirmPassword { get; set; } = string.Empty;
}