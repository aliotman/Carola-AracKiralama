using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.DtoLayer.Dtos.ReservationDtos
{
    public class CreateReservationDto
    {

        public int CarId { get; set; }

        [DataType(DataType.Date)]
        public DateTime PickupDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; }

        public int PickupLocationId { get; set; }
        public int ReturnLocationId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string LicenseNumber { get; set; }
        public string IdentityNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        public string Description { get; set; }

    }
}
