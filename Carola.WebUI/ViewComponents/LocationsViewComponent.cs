using Carola.BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Carola.WebUI.ViewComponents
{
    public class LocationsViewComponent:ViewComponent
    {
        private readonly ILocationService _locationService;

        public LocationsViewComponent(ILocationService locationService)
        {
            _locationService = locationService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var locations=await _locationService.TGetAllAsync();
            return View(locations);
        }
    }
}
