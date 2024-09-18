namespace ShopApp.Data.Entity;

public class Comment
{
    public int Id { get; set; }

    public string CommentText { get; set; } = string.Empty;

    public DateTime CommentDate { get; set; }

    public int ProductId { get; set; } 
    public Product Product { get; set; } = null!; 
    
    public int UserId { get; set; }  
    public User User { get; set; } = null!;  
}