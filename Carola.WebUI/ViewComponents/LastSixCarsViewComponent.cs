using Carola.BusinessLayer.Abstract;
using Carola.DataAccessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Carola.WebUI.ViewComponents
{
    public class LastSixCarsViewComponent : ViewComponent
    {
        private readonly ICarService _carService;

        public LastSixCarsViewComponent(ICarService carService)
        {
            _carService = carService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cars = await _carService.TGetLast6CarsAsync();
            return View(cars);
        }
    }
}