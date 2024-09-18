using Microsoft.EntityFrameworkCore;
using ShopApp.Data.Abstract;
using ShopApp.Data.Concrete.EfCore;
using ShopApp.Data.Entity;

namespace ShopApp.Data.Concrete;

public class EfOrderRepository : IOrderRepository
{   
    private ShopDbContext _context;

    public EfOrderRepository(ShopDbContext context)
    {   
        _context = context;
    }
    public IQueryable<Order> Orders => _context.Orders;

public void SaveOrder(Order order)
{
     
    _context.Orders.Add(order);
    
     
    foreach (var orderItem in order.OrderItems)
    {
         
        orderItem.OrderId = order.Id;
        _context.OrderItems.Add(orderItem);
    }

    _context.SaveChanges();
}
}