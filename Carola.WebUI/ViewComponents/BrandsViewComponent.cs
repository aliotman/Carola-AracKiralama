using Carola.BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Carola.WebUI.ViewComponents
{
    public class BrandsViewComponent:ViewComponent
    {
        private readonly IBrandService _brandService;

        public BrandsViewComponent(IBrandService brandService)
        {
            _brandService = brandService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var brands=await _brandService.TGetLast5BrandsAsync();
            return View(brands);
        }

    }
}
