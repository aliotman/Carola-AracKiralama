using Carola.DtoLayer.Dtos.ReservationDtos;
using Carola.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.DataAccessLayer.Abstract
{
    public interface IReservationDal : IGenericDal<Reservation>
    {
        Task<List<ReservationDetailDto>> GetReservationsWithDetailsAsync();
    }
}