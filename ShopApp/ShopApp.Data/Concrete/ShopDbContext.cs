using Microsoft.EntityFrameworkCore;
using ShopApp.Data.Entity;

namespace ShopApp.Data.Concrete.EfCore;

public class ShopDbContext: DbContext 
{   
    public ShopDbContext(DbContextOptions<ShopDbContext> options) : base (options)
    {
        
    }

    public DbSet<Product> Products => Set<Product>();

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Comment> Comments => Set<Comment>();

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<User> Users => Set<User>();

    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
}