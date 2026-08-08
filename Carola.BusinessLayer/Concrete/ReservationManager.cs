using Carola.BusinessLayer.Abstract;
using Carola.DataAccessLayer.Abstract;
using Carola.DtoLayer.Dtos.ReservationDtos;
using Carola.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.BusinessLayer.Concrete
{
    public class ReservationManager : IReservationService
    {
        private readonly IReservationDal _reservationDal;

        public ReservationManager(IReservationDal reservationDal)
        {
            _reservationDal = reservationDal;
        }

        public async Task TDeleteAsync(int id)
        {
            await _reservationDal.DeleteAsync(id);
        }

        public async Task<List<Reservation>> TGetAllAsync()
        {
            return await _reservationDal.GetAllAsync();
        }

        public async Task<List<ReservationDetailDto>> TGetReservationsWithDetailsAsync()
        {
            return await _reservationDal.GetReservationsWithDetailsAsync();
        }

        public async Task<Reservation> TGetByIdAsync(int id)
        {
            return await _reservationDal.GetByIdAsync(id);
        }

        public async Task TInsertAsync(Reservation entity)
        {
            await _reservationDal.InsertAsync(entity);
        }

        public async Task TUpdateAsync(Reservation entity)
        {
            await _reservationDal.UpdateAsync(entity);
        }
    }
}
