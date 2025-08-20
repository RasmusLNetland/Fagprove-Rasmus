using System.ComponentModel.DataAnnotations;

namespace ShopAppFrontend.Infrastructure.ViewModels;

public class ShoppingListViewModel
{
    [Required( ErrorMessage = "Du må gi listen et navn" )]
    public string ListName { get; set; } = string.Empty;
}