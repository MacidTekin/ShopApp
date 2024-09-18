using ShopApp.Data.Entity;

namespace ShopApp.Data.Abstract;

public interface ICategoryRepository 
{
    IQueryable<Category> Categories { get; } 

    void CreateCategory(Category category);
    void UpdateCategory(Category category); 
       
    
}