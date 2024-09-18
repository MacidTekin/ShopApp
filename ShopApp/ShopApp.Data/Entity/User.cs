namespace ShopApp.Data.Entity;

public class User
{
    public int Id { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Image { get; set; } = string.Empty;

    public List<Comment> Comments { get; set; } = new();  
    
    public List<Order> Orders { get; set; } = new();   
}