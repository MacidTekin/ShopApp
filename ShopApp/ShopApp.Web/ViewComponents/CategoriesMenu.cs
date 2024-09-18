using Microsoft.AspNetCore.Mvc;
using ShopApp.Data.Abstract;

namespace ShopApp.Web.ViewComponents;

public class CategoriesMenu : ViewComponent
{   
    private ICategoryRepository _categoryRepository;
    public CategoriesMenu(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public IViewComponentResult Invoke()
    {
        return View(_categoryRepository.Categories.ToList());
    }

}