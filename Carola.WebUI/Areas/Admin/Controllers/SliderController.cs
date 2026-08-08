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
    public class SliderController : Controller
    {
        private readonly ISliderService _sliderService;
        private readonly IValidator<Slider> _validator;

        public SliderController(ISliderService sliderService, IValidator<Slider> validator)
        {
            _sliderService = sliderService;
            _validator = validator;
        }

        public async Task<IActionResult> SliderList()
        {
            var values = await _sliderService.TGetAllAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateSlider()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSlider(Slider slider)
        {
            var result = await _validator.ValidateAsync(slider);
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                return View(slider);
            }

            await _sliderService.TInsertAsync(slider);
            return RedirectToAction("SliderList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateSlider(int id)
        {
            var value = await _sliderService.TGetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSlider(Slider slider)
        {
            var result = await _validator.ValidateAsync(slider);
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                return View(slider);
            }

            await _sliderService.TUpdateAsync(slider);
            return RedirectToAction("SliderList");
        }

        public async Task<IActionResult> DeleteSlider(int id)
        {
            await _sliderService.TDeleteAsync(id);
            return RedirectToAction("SliderList");
        }
    }
}