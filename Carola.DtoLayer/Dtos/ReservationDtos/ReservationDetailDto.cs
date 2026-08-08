using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.DtoLayer.Dtos.ReservationDtos
{
    public class ReservationDetailDto
    {
        public int ReservationId { get; set; }
        public string CarBrand { get; set; }
        public string CarModel { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerEmail { get; set; }
        public string PickupLocationName { get; set; }
        public string ReturnLocationName { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string ReservationStatus { get; set; }
        public string Description { get; set; }

    }
}
