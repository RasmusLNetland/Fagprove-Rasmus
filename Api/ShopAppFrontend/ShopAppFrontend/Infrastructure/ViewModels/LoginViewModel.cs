using System.ComponentModel.DataAnnotations;

namespace ShopAppFrontend.Infrastructure.ViewModels
{
    public class LoginViewModel
    {
        [Required( ErrorMessage = "E-post er påkrevd" )]
        [DataType( DataType.EmailAddress )]
        public string Email { get; set; } = string.Empty;

        [Required( ErrorMessage = "Passpord er påkrevd" )]
        [DataType( DataType.Password )]
        public string Password { get; set; } = string.Empty;
    }
}