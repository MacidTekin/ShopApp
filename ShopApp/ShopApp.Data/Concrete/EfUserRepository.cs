using ShopApp.Data.Abstract;
using ShopApp.Data.Concrete.EfCore;
using ShopApp.Data.Entity;

namespace ShopApp.Data.Concrete;

public class EfUserRepository : IUserRepository
{
    private readonly ShopDbContext _context;

    public EfUserRepository(ShopDbContext context)
    {
        _context = context;
    }

    public IQueryable<User> Users => _context.Users;

    public void CreateUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void UpdateUser(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }
}