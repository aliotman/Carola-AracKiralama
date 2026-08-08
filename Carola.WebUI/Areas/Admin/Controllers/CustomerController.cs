using Carola.BusinessLayer.Abstract;
using Carola.DtoLayer.Dtos.CustomerDtos;
using Carola.EntityLayer.Entities;
using Carola.WebUI.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carola.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IValidator<CreateCustomerDto> _createValidator;
        private readonly IValidator<UpdateCustomerDto> _updateValidator;

        public CustomerController(ICustomerService customerService,
            IValidator<CreateCustomerDto> createValidator,
            IValidator<UpdateCustomerDto> updateValidator)
        {
            _customerService = customerService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<IActionResult> CustomerList()
        {
            var values = await _customerService.GetAllCustomerAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CreateCustomerDto createCustomerDto)
        {
            var result = await _createValidator.ValidateAsync(createCustomerDto);
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                return View(createCustomerDto);
            }

            await _customerService.CreateCustomerAsync(createCustomerDto);
            return RedirectToAction("CustomerList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCustomer(int id)
        {
            var value = await _customerService.GetCustomerByIdAsync(id);
            var dto = new UpdateCustomerDto
            {
                CustomerId = value.CustomerId,
                FirstName = value.FirstName,
                LastName = value.LastName,
                Email = value.Email,
                Phone = value.Phone,
                DriverLicenseNumber = value.DriverLicenseNumber,
                IdentityNumber = value.IdentityNumber,
                BirthDate = value.BirthDate
            };
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCustomer(UpdateCustomerDto updateCustomerDto)
        {
            var result = await _updateValidator.ValidateAsync(updateCustomerDto);
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                return View(updateCustomerDto);
            }

            await _customerService.UpdateCustomerAsync(updateCustomerDto);
            return RedirectToAction("CustomerList");
        }

        public async Task<IActionResult> DeleteCustomer(int id)
        {
            await _customerService.DeleteCustomerAsync(id);
            return RedirectToAction("CustomerList");
        }
    }
}