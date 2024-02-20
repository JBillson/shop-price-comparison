using System.ComponentModel.DataAnnotations;
using shopping_app.Products.Enums;

namespace shopping_app.Products.Interface;

public interface IProduct
{
    [Required]
    public string? Name { get; set; }
    [Required]
    public double Price { get; set; }
    public ProductPriceType? PriceType { get; set; }
    public Shop Shop { get; set; }
    public string? Special { get; set; }
}