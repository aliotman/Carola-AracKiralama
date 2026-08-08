using Carola.BusinessLayer.Abstract;
using Carola.DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Carola.WebUI.ViewComponents
{
    public class SliderViewComponent:ViewComponent
    {
        private readonly ISliderService _sliderService;

        public SliderViewComponent(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sliders=await _sliderService.TGetAllAsync();
            return View(sliders);
        }
    }
}
