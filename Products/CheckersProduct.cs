using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;
using shopping_app.Products.Interface;

namespace shopping_app.Products;

public class CheckersProduct : IProduct
{
    public string? Name { get; set; }
    public double Price { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public IProduct.Pricing? PriceType { get; set; }
    public string? Special { get; set; }
}