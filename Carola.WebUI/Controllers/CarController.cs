using Carola.BusinessLayer.Abstract;
using Carola.DtoLayer.Dtos.CarDtos;
using Microsoft.AspNetCore.Mvc;

namespace Carola.WebUI.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarService _carService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly ILocationService _locationService;

        public CarController(ICarService carService, ICategoryService categoryService,
            IBrandService brandService, ILocationService locationService)
        {
            _carService = carService;
            _categoryService = categoryService;
            _brandService = brandService;
            _locationService = locationService;
        }

        public async Task<IActionResult> List(int? locationId, int? returnLocationId, DateTime? startDate, DateTime? endDate, int? categoryId, string transmissionType, decimal? minPrice, decimal? maxPrice, string brand, int page = 1)
        {
            var cars = await _carService.TGetAvailableCarsAsync(locationId, startDate, endDate);

            if (categoryId.HasValue)
                cars = cars.Where(c => c.CategoryId == categoryId).ToList();

            if (!string.IsNullOrEmpty(transmissionType))
                cars = cars.Where(c => c.TransmissionType == transmissionType).ToList();

            if (minPrice.HasValue)
                cars = cars.Where(c => c.DailyPrice >= minPrice).ToList();

            if (maxPrice.HasValue)
                cars = cars.Where(c => c.DailyPrice <= maxPrice).ToList();

            if (!string.IsNullOrEmpty(brand))
                cars = cars.Where(c => c.Brand == brand).ToList();

            int pageSize = 6;
            int totalPages = (int)Math.Ceiling(cars.Count / (double)pageSize);
            if (page < 1) page = 1;
            if (totalPages > 0 && page > totalPages) page = totalPages;
            var pagedCars = cars.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var categories = await _categoryService.TGetAllAsync();
            var brands = await _brandService.TGetAllAsync();

            ViewBag.Categories = categories;
            ViewBag.Brands = brands;
            ViewBag.Locations = await _locationService.TGetAllAsync();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.LocationId = locationId;
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            ViewBag.CategoryId = categoryId;
            ViewBag.TransmissionType = transmissionType;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.Brand = brand;
            ViewBag.ReturnLocationId = returnLocationId;

            return View(pagedCars);
        }
    }
}
