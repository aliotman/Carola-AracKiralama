using Carola.EntityLayer.Entities;
using FluentValidation;

namespace Carola.BusinessLayer.ValidationRules
{
    public class LocationValidator : AbstractValidator<Location>
    {
        public LocationValidator()
        {
            RuleFor(x => x.LocationName)
                .NotEmpty().WithMessage("Şube adı boş geçilemez.")
                .Length(2, 60).WithMessage("Şube adı 2 ile 60 karakter arasında olmalıdır.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("Şehir boş geçilemez.")
                .Length(2, 40).WithMessage("Şehir 2 ile 40 karakter arasında olmalıdır.")
                .Matches(@"^[a-zA-ZğüşöçıİĞÜŞÖÇ\s]+$")
                .WithMessage("Şehir adı yalnızca harflerden oluşmalıdır.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Adres boş geçilemez.")
                .Length(10, 250).WithMessage("Adres 10 ile 250 karakter arasında olmalıdır.");

            RuleFor(x => x.AuthorizedPerson)
                .NotEmpty().WithMessage("Yetkili kişi boş geçilemez.")
                .Length(3, 60).WithMessage("Yetkili kişi 3 ile 60 karakter arasında olmalıdır.");
        }
    }
}
