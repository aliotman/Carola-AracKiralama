using Carola.BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Carola.WebUI.ViewComponents
{
    public class CarTypesViewComponent:ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public CarTypesViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories=await _categoryService.TGetLast5CategoriesAsync();
            return View(categories);
        } 
    }
}
