using Carola.DataAccessLayer.Abstract;
using Carola.DataAccessLayer.Concrete;
using Carola.DataAccessLayer.Repository;
using Carola.DtoLayer.Dtos.ReservationDtos;
using Carola.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.DataAccessLayer.EntityFramework
{
    public class EFReservationDal : GenericRepository<Reservation>, IReservationDal
    {
        public EFReservationDal(CarolaContext context) : base(context)
        {
        }

        public async Task<List<ReservationDetailDto>> GetReservationsWithDetailsAsync()
        {

            var values = await (from reservation in _context.Reservations
                                join car in _context.Cars
                                    on reservation.CarId equals car.CarId
                                join customer in _context.Customers
                                    on reservation.CustomerId equals customer.CustomerId
                                join pickupLocation in _context.Locations
                                    on reservation.PickupLocationId equals pickupLocation.LocationId
                                join returnLocation in _context.Locations
                                    on reservation.ReturnLocationId equals returnLocation.LocationId
                                orderby reservation.ReservationId descending
                                select new ReservationDetailDto
                                {
                                    ReservationId = reservation.ReservationId,
                                    CarBrand = car.Brand,
                                    CarModel = car.Model,
                                    CustomerFirstName = customer.FirstName,
                                    CustomerLastName = customer.LastName,
                                    CustomerEmail = customer.Email,
                                    PickupLocationName = pickupLocation.LocationName,
                                    ReturnLocationName = returnLocation.LocationName,
                                    PickupDate = reservation.PickupDate,
                                    ReturnDate = reservation.ReturnDate,
                                    TotalPrice = reservation.TotalPrice,
                                    ReservationStatus = reservation.ReservationStatus,
                                    Description = reservation.Description
                                }).ToListAsync();

            return values;
        }
    }
}
