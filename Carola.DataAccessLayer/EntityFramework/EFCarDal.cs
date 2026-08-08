using Carola.DataAccessLayer.Abstract;
using Carola.DataAccessLayer.Concrete;
using Carola.DataAccessLayer.Repository;
using Carola.DtoLayer.Dtos.CarDtos;
using Carola.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.DataAccessLayer.EntityFramework
{
    public class EFCarDal : GenericRepository<Car>, ICarDal
    {
        public EFCarDal(CarolaContext context) : base(context)
        {
        }

        public async Task<List<Car>> GetAllCarsWithCategoryAsync()
        {
            var values = await _context.Cars
                .Include(c => c.Category)
                .Include(c => c.Location)
                .ToListAsync();
            return values;
        }

        public async Task<List<CarListDto>> GetAvailableCarsAsync(int? locationId, DateTime? startDate, DateTime? endDate)
        {

            var query = _context.Cars
                .Include(c => c.Category)
                .Where(c => c.IsAvailable);

            if (locationId.HasValue && locationId.Value > 0)
                query = query.Where(c => c.LocationId == locationId.Value);

            if (startDate.HasValue && endDate.HasValue)
            {
                var reservedCarIds = await _context.Reservations
                    .Where(r => (r.ReservationStatus == "Beklemede" || r.ReservationStatus == "Onaylandı")
                        && r.PickupDate < endDate.Value
                        && r.ReturnDate > startDate.Value)
                    .Select(r => r.CarId)
                    .ToListAsync();

                query = query.Where(c => !reservedCarIds.Contains(c.CarId));
            }

            return await query
                .Select(c => new CarListDto
                {
                    CarId = c.CarId,
                    Brand = c.Brand,
                    Model = c.Model,
                    ModelYear = c.ModelYear,
                    DailyPrice = c.DailyPrice,
                    SeatCount = c.SeatCount,
                    FuelType = c.FuelType,
                    TransmissionType = c.TransmissionType,
                    ImageUrl = c.ImageUrl,
                    CategoryId = c.CategoryId,
                    CategoryName = c.Category.CategoryName
                })
                .ToListAsync();
        }

        public async Task<bool> IsCarAvailableAsync(int carId, DateTime startDate, DateTime endDate)
        {

            var car = await _context.Cars.FirstOrDefaultAsync(c => c.CarId == carId);
            if (car == null || !car.IsAvailable)
                return false;

            var hasConflict = await _context.Reservations.AnyAsync(r =>
                r.CarId == carId
                && (r.ReservationStatus == "Beklemede" || r.ReservationStatus == "Onaylandı")
                && r.PickupDate < endDate
                && r.ReturnDate > startDate);

            return !hasConflict;
        }

        public async Task<List<CarListDto>> GetLast6CarsAsync()
        {
            var values = await _context.Cars.OrderByDescending(x => x.CarId).Take(6).Select(x => new CarListDto
            {
                CarId = x.CarId,
                Brand = x.Brand,
                Model = x.Model,
                ModelYear = x.ModelYear,
                DailyPrice = x.DailyPrice,
                SeatCount = x.SeatCount,
                FuelType = x.FuelType,
                TransmissionType = x.TransmissionType,
                ImageUrl = x.ImageUrl,
                CategoryId=x.CategoryId,
                CategoryName = x.Category.CategoryName
            }).ToListAsync();

            return values;
        }
    }
}
