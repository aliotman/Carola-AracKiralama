using Carola.BusinessLayer.Abstract;
using Carola.DtoLayer.Dtos.CustomerDtos;
using Carola.DtoLayer.Dtos.ReservationDtos;
using Carola.EntityLayer.Entities;
using Carola.WebUI.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Text.RegularExpressions;
using Tesseract;

namespace Carola.WebUI.Controllers
{
    public class BookingController : Controller
    {
        private readonly ICarService _carService;
        private readonly ILocationService _locationService;
        private readonly IReservationService _reservationService;
        private readonly ICustomerService _customerService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IValidator<CreateReservationDto> _reservationValidator;

        public BookingController(ICarService carService, ILocationService locationService,
            IReservationService reservationService, ICustomerService customerService,
            IWebHostEnvironment hostEnvironment, IValidator<CreateReservationDto> reservationValidator)
        {
            _carService = carService;
            _locationService = locationService;
            _reservationService = reservationService;
            _customerService = customerService;
            _hostEnvironment = hostEnvironment;
            _reservationValidator = reservationValidator;

        }

        private async Task<bool> PrepareCreateViewAsync(CreateReservationDto dto)
        {
            var cars = await _carService.TGetAllCarsWithCategoryAsync();
            var car = cars.FirstOrDefault(c => c.CarId == dto.CarId);

            if (car == null)
                return false;

            int totalDays = Math.Max(1, (dto.ReturnDate.Date - dto.PickupDate.Date).Days);

            ViewBag.Car = car;
            ViewBag.Locations = await _locationService.TGetAllAsync();
            ViewBag.TotalDays = totalDays;
            ViewBag.TotalPrice = totalDays * car.DailyPrice;

            return true;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int carId, DateTime startDate, DateTime endDate, int locationId, int returnLocationId)
        {
            var dto = new CreateReservationDto
            {
                CarId = carId,
                PickupDate = startDate,
                ReturnDate = endDate,
                PickupLocationId = locationId,
                ReturnLocationId = returnLocationId
            };

            if (!await PrepareCreateViewAsync(dto))
                return RedirectToAction("List", "Car");

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateReservationDto dto)
        {
            var validation = await _reservationValidator.ValidateAsync(dto);

            if (!validation.IsValid)
            {
                validation.AddToModelState(ModelState);

                if (!await PrepareCreateViewAsync(dto))
                    return RedirectToAction("List", "Car");

                return View(dto);
            }

            var car = await _carService.TGetByIdAsync(dto.CarId);
            int totalDays = Math.Max(1, (dto.ReturnDate.Date - dto.PickupDate.Date).Days);
            decimal totalPrice = totalDays * car.DailyPrice;

            // Müşteri var mı kontrol et
            var customers = await _customerService.TGetAllAsync();
            var existingCustomer = customers.FirstOrDefault(c => c.Email == dto.Email);

            int customerId;

            if (existingCustomer == null)
            {
                // Yeni müşteri oluştur
                var newCustomer = new Customer
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    DriverLicenseNumber = dto.LicenseNumber,
                    IdentityNumber = dto.IdentityNumber,
                    BirthDate = dto.BirthDate.Value
                };
                await _customerService.TInsertAsync(newCustomer);

                var allCustomers = await _customerService.TGetAllAsync();
                customerId = allCustomers.First(c => c.Email == dto.Email).CustomerId;
            }
            else
            {
                customerId = existingCustomer.CustomerId;
            }

            // Rezervasyon oluştur
            var reservation = new Reservation
            {
                CarId = dto.CarId,
                CustomerId = customerId,
                PickupDate = dto.PickupDate,
                ReturnDate = dto.ReturnDate,
                PickupLocationId = dto.PickupLocationId,
                ReturnLocationId = dto.ReturnLocationId,
                TotalPrice = totalPrice,
                ReservationStatus = "Beklemede",
                Description = dto.Description ?? ""
            };

            await _reservationService.TInsertAsync(reservation);

            TempData["OwnedReservationId"] = reservation.ReservationId;

            return RedirectToAction("Confirmation", new { reservationId = reservation.ReservationId });
        }
        private const long MaxDosyaBoyutu = 5 * 1024 * 1024;
        private static readonly string[] IzinliUzantilar = { ".jpg", ".jpeg", ".png", ".webp", ".bmp" };

        [HttpPost]
        public async Task<IActionResult> ProcessLicense(IFormFile licenseImage)
        {
            if (licenseImage == null || licenseImage.Length == 0)
                return Json(new { success = false, message = "Dosya yüklenemedi." });

            if (licenseImage.Length > MaxDosyaBoyutu)
                return Json(new { success = false, message = "Dosya boyutu en fazla 5 MB olabilir." });

            var uzanti = Path.GetExtension(licenseImage.FileName)?.ToLowerInvariant();
            if (string.IsNullOrEmpty(uzanti) || !IzinliUzantilar.Contains(uzanti))
                return Json(new { success = false, message = "Yalnızca JPG, PNG, WEBP veya BMP dosyası yükleyebilirsiniz." });

            if (licenseImage.ContentType == null || !licenseImage.ContentType.StartsWith("image/"))
                return Json(new { success = false, message = "Yüklenen dosya bir görsel değil." });

            try
            {
                var tessDataPath = Path.Combine(_hostEnvironment.WebRootPath, "tessdata");

                using var engine = new TesseractEngine(tessDataPath, "tur+eng", EngineMode.Default);

                using var ms = new MemoryStream();
                await licenseImage.CopyToAsync(ms);
                ms.Position = 0;

                using var img = Pix.LoadFromMemory(ms.ToArray());
                using var page = engine.Process(img);

                var text = page.GetText();

                // Metinden ehliyet no ve doğum tarihi çekmeye çalış
                var licenseNumber = ExtractLicenseNo(text);
                var birthDate = ExtractBirthDate(text);

                return Json(new
                {
                    success = true,
                    licenseNumber = ExtractLicenseNo(text),
                    identityNumber = ExtractIdentityNumber(text),
                    birthDate = ExtractBirthDate(text),
                    firstName = ExtractFirstName(text),
                    lastName = ExtractLastName(text),
                    rawText = text
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private string ExtractLicenseNo(string text)
        {
            // 5. alan: sürücü belgesi numarası
            var match = Regex.Match(text, @"5[:\s\.]*(\d{5,10})");
            if (match.Success) return match.Groups[1].Value.Trim();
            return "";
        }

        private string ExtractIdentityNumber(string text)
        {
            // 4d alanı: TC kimlik no (11 hane)
            var match = Regex.Match(text, @"4d[:\s\.]*(\d{11})");
            if (match.Success) return match.Groups[1].Value.Trim();

            // Alternatif: 11 haneli rakam dizisi
            match = Regex.Match(text, @"\b(\d{11})\b");
            return match.Success ? match.Groups[1].Value : "";
        }

        private string ExtractBirthDate(string text)
        {
            // 3. alan: doğum tarihi DD.MM.YYYY
            var match = Regex.Match(text, @"3[:\s\.]*(\d{2}[.]\d{2}[.]\d{4})");
            if (match.Success) return match.Groups[1].Value.Trim();

            // Alternatif: direkt tarih formatı
            match = Regex.Match(text, @"\b(\d{2}[.\/]\d{2}[.\/]\d{4})\b");
            return match.Success ? match.Groups[1].Value : "";
        }
        private string ExtractFirstName(string text)
        {
            // 2. alan: ad
            var match = Regex.Match(text, @"2[:\s\.]*([A-ZÇĞİÖŞÜa-zçğışöşü]+)");
            return match.Success ? match.Groups[1].Value.Trim() : "";
        }

        private string ExtractLastName(string text)
        {
            // 1. alan: soyad
            var match = Regex.Match(text, @"1[:\s\.]*([A-ZÇĞİÖŞÜa-zçğışöşü]+)");
            return match.Success ? match.Groups[1].Value.Trim() : "";
        }
        public async Task<IActionResult> Confirmation(int reservationId)
        {
            var sahiplik = TempData.Peek("OwnedReservationId");
            if (sahiplik == null || Convert.ToInt32(sahiplik) != reservationId)
                return RedirectToAction("Index", "Home");

            var reservation = await _reservationService.TGetByIdAsync(reservationId);
            if (reservation == null)
                return RedirectToAction("Index", "Home");

            ViewBag.ReservationId = reservation.ReservationId;
            ViewBag.PickupDate = reservation.PickupDate.ToString("dd.MM.yyyy");
            ViewBag.ReturnDate = reservation.ReturnDate.ToString("dd.MM.yyyy");
            ViewBag.TotalPrice = reservation.TotalPrice;
            ViewBag.Status = reservation.ReservationStatus;

            return View();
        }
    }
}
