using Carola.BusinessLayer.Abstract;
using Carola.EntityLayer.Entities;
using Carola.WebUI.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Carola.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CarController : Controller
    {
        private readonly ICarService _carService;
        private readonly ICategoryService _categoryService;
        private readonly ILocationService _locationService;
        private readonly IValidator<Car> _validator;

        public CarController(ICarService carService, ICategoryService categoryService,
            ILocationService locationService, IValidator<Car> validator)
        {
            _carService = carService;
            _categoryService = categoryService;
            _locationService = locationService;
            _validator = validator;
        }

        public async Task<IActionResult> CarList()
        {
            var values = await _carService.TGetAllCarsWithCategoryAsync();
            return View(values);
        }

        private async Task FillDropdownsAsync(Car car = null)
        {
            ViewBag.Categories = new SelectList(await _categoryService.TGetAllAsync(),
                "CategoryId", "CategoryName", car?.CategoryId);
            ViewBag.Locations = new SelectList(await _locationService.TGetAllAsync(),
                "LocationId", "LocationName", car?.LocationId);
        }

        [HttpGet]
        public async Task<IActionResult> CreateCar()
        {
            await FillDropdownsAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCar(Car car)
        {
            var result = await _validator.ValidateAsync(car);
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                await FillDropdownsAsync(car);
                return View(car);
            }

            await _carService.TInsertAsync(car);
            return RedirectToAction("CarList", "Car", new { area = "Admin" });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCar(int id)
        {
            var value = await _carService.TGetByIdAsync(id);
            await FillDropdownsAsync(value);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCar(Car car)
        {
            var result = await _validator.ValidateAsync(car);
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                await FillDropdownsAsync(car);
                return View(car);
            }

            await _carService.TUpdateAsync(car);
            return RedirectToAction("CarList", "Car", new { area = "Admin" });
        }

        public async Task<IActionResult> DeleteCar(int id)
        {
            await _carService.TDeleteAsync(id);
            return RedirectToAction("CarList", "Car", new { area = "Admin" });
        }
    }
}
