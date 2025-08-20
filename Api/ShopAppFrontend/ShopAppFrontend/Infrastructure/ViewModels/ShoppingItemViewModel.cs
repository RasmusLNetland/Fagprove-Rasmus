using System.ComponentModel.DataAnnotations;

namespace ShopAppFrontend.Infrastructure.ViewModels
{
    public class ShoppingItemViewModel
    {
        [Required( ErrorMessage = "Navn på vare kan ikke være tomt" )]
        public string Name { get; set; } = string.Empty;

        [Range( 1, int.MaxValue, ErrorMessage = "Antall må være minst 1" )]
        public int Count { get; set; } = 1;
    }
}