using Carola.DtoLayer.Dtos.CustomerDtos;
using Carola.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.BusinessLayer.Abstract
{
    public interface ICustomerService:IGenericService<Customer>
    {
        Task<List<ResultCustomerDto>> GetAllCustomerAsync();
        Task<GetCustomerByIdDto> GetCustomerByIdAsync(int id);
        Task CreateCustomerAsync(CreateCustomerDto createCustomerDto);
        Task UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto);
        Task DeleteCustomerAsync(int id);
    }
}
