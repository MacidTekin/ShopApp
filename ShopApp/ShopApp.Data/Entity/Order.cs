using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApp.Data.Entity;

public class Order
{
public int Id { get; set; }
    
    public DateTime OrderDate { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string City { get; set; } = string.Empty;
    
    public string Phone { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string AdressLine { get; set; } = string.Empty;

    // Foreign Key - Siparişi veren kullanıcı
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; } = null!;

    // Siparişe ait ürünler (OrderItems)
    public List<OrderItem> OrderItems { get; set; } = new();

    
}
public class OrderItem
{
     public int Id { get; set; }
    
    // Foreign Key - Sipariş ID
    public int OrderId { get; set; }

    [ForeignKey("OrderId")]
    public Order? Order { get; set; } = new();

    // Foreign Key - Ürün ID
    public int ProductId { get; set; }

    [ForeignKey("ProductId")]
    public Product? Product { get; set; } = null!;

    public double Price { get; set; }
    
    public int Quantity { get; set; }
}
