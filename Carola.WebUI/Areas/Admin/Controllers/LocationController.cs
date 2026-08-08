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
    public class LocationController : Controller
    {
        private readonly ILocationService _locationService;
        private readonly IValidator<Location> _validator;

        public LocationController(ILocationService locationService, IValidator<Location> validator)
        {
            _locationService = locationService;
            _validator = validator;
        }

        public async Task<IActionResult> LocationList()
        {
            var values=await _locationService.TGetAllAsync();
            return View(values);
        }
        public IActionResult CreateLocation()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>CreateLocation(Location location)
        {
            var result = await _validator.ValidateAsync(location);
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                return View(location);
            }

            await _locationService.TInsertAsync(location);
            return RedirectToAction("LocationList");
        }
        public async Task<IActionResult> DeleteLocation(int id)
        {
            await _locationService.TDeleteAsync(id);
            return RedirectToAction("LocationList");
        }
        [HttpGet]
        public async Task<IActionResult>UpdateLocation(int id)
        {
            var value = await _locationService.TGetByIdAsync(id);
            return View(value);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateLocation(Location location)
        {
            var result = await _validator.ValidateAsync(location);
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                return View(location);
            }

            await _locationService.TUpdateAsync(location);
            return RedirectToAction("LocationList");
        }
    }
}
