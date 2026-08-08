using AutoMapper;
using Carola.BusinessLayer.Abstract;
using Carola.DataAccessLayer.Abstract;
using Carola.DtoLayer.Dtos.CustomerDtos;
using Carola.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.BusinessLayer.Concrete
{
    public class CustomerManager : ICustomerService
    {
        private readonly ICustomerDal _customerDal;
        private readonly IMapper _mapper;

        public CustomerManager(ICustomerDal customerDal, IMapper mapper)
        {
            _customerDal = customerDal;
            _mapper = mapper;
        }


        // Generic metodlar
        public async Task TDeleteAsync(int id)
            => await _customerDal.DeleteAsync(id);

        public async Task<List<Customer>> TGetAllAsync()
            => await _customerDal.GetAllAsync();

        public async Task<Customer> TGetByIdAsync(int id)
            => await _customerDal.GetByIdAsync(id);

        public async Task TInsertAsync(Customer entity)
            => await _customerDal.InsertAsync(entity);

        public async Task TUpdateAsync(Customer entity)
            => await _customerDal.UpdateAsync(entity);


        // Dto bazlı metodlar
        public async Task<List<ResultCustomerDto>> GetAllCustomerAsync()
        {
            var values = await _customerDal.GetAllAsync();
            return _mapper.Map<List<ResultCustomerDto>>(values);
        }

        public async Task<GetCustomerByIdDto> GetCustomerByIdAsync(int id)
        {
            var value = await _customerDal.GetByIdAsync(id);
            return _mapper.Map<GetCustomerByIdDto>(value);
        }

        public async Task CreateCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            var value = _mapper.Map<Customer>(createCustomerDto);
            await _customerDal.InsertAsync(value);
        }

        public async Task UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto)
        {
            var value = _mapper.Map<Customer>(updateCustomerDto);
            await _customerDal.UpdateAsync(value);
        }

        public async Task DeleteCustomerAsync(int id)
        {
            await _customerDal.DeleteAsync(id);
        }
    }
}
