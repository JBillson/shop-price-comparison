using System.ComponentModel.DataAnnotations;

namespace shopping_app.Products.Interface;

public interface IProduct
{
    [Required]
    public string? Name { get; set; }
    [Required]
    public double Price { get; set; }
    public Pricing? PriceType { get; set; }
    public string? Special { get; set; }

    enum Pricing
    {
        PerItem,
        PerKg
    }
}