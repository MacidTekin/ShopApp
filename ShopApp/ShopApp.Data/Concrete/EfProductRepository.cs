using Microsoft.EntityFrameworkCore;
using ShopApp.Data.Abstract;
using ShopApp.Data.Concrete.EfCore;
using ShopApp.Data.Entity;
using System.Threading.Tasks;

namespace ShopApp.Data.Concrete
{
    public class EfProductRepository : IProductRepository
    {
        private readonly ShopDbContext _context;

        public EfProductRepository(ShopDbContext context)
        {
            _context = context;
        }

        public IQueryable<Product> Products => _context.Products;

        // Senkron bırakılan Create metodu
        public void CreateProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products
                        .Include(p => p.Categories) 
                        .FirstOrDefaultAsync(p => p.Id == id);
                        
            if(product == null)
            {
              throw new Exception("Ürün bulunamadı.");  
            } 

            product.Categories.Clear();
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }


        // Asenkron Edit metodu
        public async Task EditProductAsync(Product product, List<int> selectedCategoryIds)
    {
        var entity = await _context.Products
            .Include(p => p.Categories)
            .FirstOrDefaultAsync(p => p.Id == product.Id);

        if (entity == null) throw new Exception("Ürün bulunamadı.");

        // Ürün bilgilerini güncelle
        entity.Name = product.Name;
        entity.Description = product.Description;
        entity.Url = product.Url;
        entity.Price = product.Price;
        entity.TotalStock = product.TotalStock;
        entity.Image = product.Image;
        entity.Content = product.Content;

        // Kategorileri güncelleme
        var existingCategories = entity.Categories.Select(c => c.Id).ToList();
        
        // Yeni kategoriler ekle
        var newCategories = selectedCategoryIds.Except(existingCategories).ToList();
        foreach (var categoryId in newCategories)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            if (category != null)
            {
                entity.Categories.Add(category);
            }
        }

        // Eski kategorileri çıkar
        var categoriesToRemove = existingCategories.Except(selectedCategoryIds).ToList();
        foreach (var categoryId in categoriesToRemove)
        {
            var category = entity.Categories.FirstOrDefault(c => c.Id == categoryId);
            if (category != null)
            {
                entity.Categories.Remove(category);
            }
        }

        await _context.SaveChangesAsync();
    }

        public async Task EditProductAsync(int id)
        {
            var product = await _context.Products.Include(c => c.Categories).FirstOrDefaultAsync(p => p.Id == id);

            if(product == null) throw new Exception("Ürün bulunamadı.");

            product.Categories.Clear();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductStockAsync(Product product)
        {   var entity = await _context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
            if(entity != null)
            {
                entity.TotalStock = product.TotalStock;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Ürün bulunamadı.");
            }
        }
    }
}
