using ShopApp.Data.Entity;

namespace ShopApp.Data.Abstract;

public interface IProductRepository 
{
    IQueryable<Product> Products { get; }  

    void CreateProduct(Product product);

    Task EditProductAsync(Product product, List<int> selectedCategoryIds);

    Task DeleteProductAsync(int id);  

    Task UpdateProductStockAsync(Product product);
}