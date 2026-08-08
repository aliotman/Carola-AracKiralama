using Carola.BusinessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carola.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminLayoutController : Controller
    {
        private readonly ICarService _carService;
        private readonly ICustomerService _customerService;
        private readonly IReservationService _reservationService;
        private readonly ILocationService _locationService;

        public AdminLayoutController(ICarService carService, ICustomerService customerService, IReservationService reservationService, ILocationService locationService)
        {
            _carService = carService;
            _customerService = customerService;
            _reservationService = reservationService;
            _locationService = locationService;
        }

        public async Task<IActionResult> Index()
        {
            var cars = await _carService.TGetAllAsync();
            var customers = await _customerService.TGetAllAsync();
            var reservations = await _reservationService.TGetAllAsync();
            var locations = await _locationService.TGetAllAsync();

            ViewBag.TotalCars = cars.Count;
            ViewBag.AvailableCars = cars.Count(c => c.IsAvailable);
            ViewBag.RentedCars = cars.Count(c => !c.IsAvailable);
            ViewBag.TotalCustomers = customers.Count;
            ViewBag.TotalReservations = reservations.Count;
            ViewBag.PendingReservations = reservations.Count(r => r.ReservationStatus == "Beklemede");
            ViewBag.ApprovedReservations = reservations.Count(r => r.ReservationStatus == "Onaylandı");
            ViewBag.RejectedReservations = reservations.Count(r => r.ReservationStatus == "Reddedildi");
            ViewBag.TotalLocations = locations.Count;
            ViewBag.TotalRevenue = reservations.Where(r => r.ReservationStatus == "Onaylandı").Sum(r => r.TotalPrice);

            return View("Dashboard");
        }
    }
}