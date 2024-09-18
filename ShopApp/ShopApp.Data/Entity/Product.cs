
namespace ShopApp.Data.Entity;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public List<string> Content { get; set; } = new List<string>(); 

    public double Price { get; set; }

    public string Image { get; set; } = string.Empty;

    public int TotalStock { get; set; }

    public List<Category> Categories { get; set; } = new(); 

    public List<Comment> Comments { get; set; } = new(); 

     public List<OrderItem> OrderItems { get; set; } = new();


}