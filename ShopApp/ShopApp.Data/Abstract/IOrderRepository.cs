using ShopApp.Data.Entity;

namespace ShopApp.Data.Abstract;

public interface IOrderRepository 
{
    IQueryable<Order> Orders { get; }

    void SaveOrder(Order order);
}