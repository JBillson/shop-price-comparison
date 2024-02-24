using System.ComponentModel.DataAnnotations;
using shopping_app.Products.Enums;

namespace shopping_app.Products.Interface;

public class Product
{
    public Shop Shop { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public double Price { get; set; }
    public PriceType? PriceType { get; set; }
    public string? Special { get; set; }
}