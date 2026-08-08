using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.DtoLayer.Dtos.CustomerDtos
{
    public class ResultCustomerDto
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string Initials { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
