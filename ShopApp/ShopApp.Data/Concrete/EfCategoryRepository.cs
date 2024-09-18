using ShopApp.Data.Abstract;
using ShopApp.Data.Concrete.EfCore;
using ShopApp.Data.Entity;

namespace ShopApp.Data.Concrete;

public class EfCategoryRepository : ICategoryRepository
{   
    private readonly ShopDbContext _context;

    public EfCategoryRepository(ShopDbContext context)
    {
        _context = context;
    }
    public IQueryable<Category> Categories => _context.Categories; // Categories ları geriye gönderebileceğiz fakat tolist demedik veritabanı sorgusu yapmıyoruz.

    public void CreateCategory(Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChanges();
    }

    public void UpdateCategory(Category category)
    {
        _context.Categories.Update(category);
        _context.SaveChanges();
    }

 
}