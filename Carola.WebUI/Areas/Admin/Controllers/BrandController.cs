using Carola.BusinessLayer.Abstract;
using Carola.EntityLayer.Entities;
using Carola.WebUI.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carola.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;
        private readonly IValidator<Brand> _validator;

        public BrandController(IBrandService brandService, IValidator<Brand> validator)
        {
            _brandService = brandService;
            _validator = validator;
        }

        public async Task<IActionResult> BrandList()
        {
            var values = await _brandService.TGetAllAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateBrand()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateBrand(Brand brand)
        {
            var result = await _validator.ValidateAsync(brand);
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                return View(brand);
            }

            await _brandService.TInsertAsync(brand);
            return RedirectToAction("BrandList");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateBrand(int id)
        {
            var value = await _brandService.TGetByIdAsync(id);
            return View(value);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBrand(Brand brand)
        {
            var result = await _validator.ValidateAsync(brand);
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                return View(brand);
            }

            await _brandService.TUpdateAsync(brand);
            return RedirectToAction("BrandList");
        }
        public async Task <IActionResult> DeleteBrand(int id)
        {
            await _brandService.TDeleteAsync(id);
            return RedirectToAction("BrandList");
        }
    }
}
