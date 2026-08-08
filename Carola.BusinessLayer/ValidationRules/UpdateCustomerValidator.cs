using Carola.DtoLayer.Dtos.CustomerDtos;
using FluentValidation;
using System;

namespace Carola.BusinessLayer.ValidationRules
{
    public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerDto>
    {
        public UpdateCustomerValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("Geçersiz müşteri kaydı.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Ad alanı boş geçilemez.")
                .Length(2, 50).WithMessage("Ad 2 ile 50 karakter arasında olmalıdır.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Soyad alanı boş geçilemez.")
                .Length(2, 50).WithMessage("Soyad 2 ile 50 karakter arasında olmalıdır.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta alanı boş geçilemez.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.")
                .MaximumLength(100).WithMessage("E-posta en fazla 100 karakter olabilir.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefon alanı boş geçilemez.")
                .Matches(@"^[0-9\s\+\(\)\-]{10,20}$")
                .WithMessage("Geçerli bir telefon numarası giriniz.");

            RuleFor(x => x.DriverLicenseNumber)
                .NotEmpty().WithMessage("Sürücü belgesi numarası boş geçilemez.")
                .Length(5, 20).WithMessage("Sürücü belgesi numarası 5 ile 20 karakter arasında olmalıdır.");

            RuleFor(x => x.IdentityNumber)
                .NotEmpty().WithMessage("TC Kimlik No boş geçilemez.")
                .Length(11).WithMessage("TC Kimlik No tam olarak 11 haneli olmalıdır.")
                .Matches(@"^\d{11}$").WithMessage("TC Kimlik No yalnızca rakamlardan oluşmalıdır.")
                .Must(tc => !string.IsNullOrEmpty(tc) && tc[0] != '0')
                .WithMessage("TC Kimlik No 0 ile başlayamaz.");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("Doğum tarihi boş geçilemez.")
                .Must(CustomerAgeRule.BeAtLeast18)
                .WithMessage("Müşteri 18 yaşından küçük olamaz.");
        }
    }
}
