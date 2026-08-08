using Carola.BusinessLayer.Abstract;
using Carola.EntityLayer.Entities;
using Carola.WebUI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carola.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly ICarService _carService;
        private readonly ICustomerService _customerService;
        private readonly MailService _mailService;

        public ReservationController(IReservationService reservationService, ICarService carService, ICustomerService customerService, MailService mailService)
        {
            _reservationService = reservationService;
            _carService = carService;
            _customerService = customerService;
            _mailService = mailService;
        }

        public async Task<IActionResult> ReservationList()
        {
            var values = await _reservationService.TGetReservationsWithDetailsAsync();
            return View(values);
        }

        public async Task<IActionResult> PendingList()
        {
            var values = await _reservationService.TGetReservationsWithDetailsAsync();
            var pending = values.Where(r => r.ReservationStatus == "Beklemede").ToList();
            return View(pending);
        }

        public async Task<IActionResult> ApproveReservation(int id)
        {
            var reservation = await _reservationService.TGetByIdAsync(id);
            reservation.ReservationStatus = "Onaylandı";
            await _reservationService.TUpdateAsync(reservation);

            // Aracı müsait değil olarak işaretle
            var car = await _carService.TGetByIdAsync(reservation.CarId);
            car.IsAvailable = false;
            await _carService.TUpdateAsync(car);

            // Müşteriyi bul ve mail gönder
            var customer = await _customerService.TGetByIdAsync(reservation.CustomerId);
            var couponCode = _mailService.GenerateCouponCode();

            await _mailService.SendApprovalMailAsync(
                customer.Email,
                customer.FirstName + " " + customer.LastName,
                reservation.ReservationId,
                reservation.TotalPrice,
                reservation.PickupDate.ToString("dd.MM.yyyy"),
                reservation.ReturnDate.ToString("dd.MM.yyyy"),
                couponCode
            );

            return RedirectToAction("PendingList", "Reservation", new { area = "Admin" });
        }

        public async Task<IActionResult> RejectReservation(int id)
        {
            var reservation = await _reservationService.TGetByIdAsync(id);
            reservation.ReservationStatus = "Reddedildi";
            await _reservationService.TUpdateAsync(reservation);

            return RedirectToAction("PendingList", "Reservation", new { area = "Admin" });
        }

        public async Task<IActionResult> DeleteReservation(int id)
        {
            await _reservationService.TDeleteAsync(id);
            return RedirectToAction("ReservationList", "Reservation", new { area = "Admin" });
        }
    }
}