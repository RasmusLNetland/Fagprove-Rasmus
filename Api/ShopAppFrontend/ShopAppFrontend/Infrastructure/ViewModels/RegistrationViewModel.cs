using System.ComponentModel.DataAnnotations;

namespace ShopAppFrontend.Infrastructure.ViewModels;

public class RegistrationViewModel
{
    [Required( ErrorMessage = "E-post er påkrevd" )]
    [DataType( DataType.EmailAddress )]

    public string Email { get; set; } = string.Empty;

    [Required( ErrorMessage = "Navn er påkrevd")]
    public string FullName { get; set; } = string.Empty;

    [Required( ErrorMessage = "Passord er påkrevd")]
    [DataType( DataType.Password )]
    public string Password { get; set; } = string.Empty;
}