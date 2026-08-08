using Carola.EntityLayer.Entities;
using FluentValidation;
using System;

namespace Carola.BusinessLayer.ValidationRules
{
    public class ReservationValidator : AbstractValidator<Reservation>
    {
        private static readonly string[] GecerliDurumlar =
            { "Beklemede", "Onaylandı", "Reddedildi" };

        public ReservationValidator()
        {
            RuleFor(x => x.CarId)
                .GreaterThan(0).WithMessage("Geçerli bir araç seçilmelidir.");

            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("Geçerli bir müşteri seçilmelidir.");

            RuleFor(x => x.PickupLocationId)
                .GreaterThan(0).WithMessage("Alış şubesi seçilmelidir.");

            RuleFor(x => x.ReturnLocationId)
                .GreaterThan(0).WithMessage("İade şubesi seçilmelidir.");

            RuleFor(x => x.PickupDate)
                .NotEmpty().WithMessage("Alış tarihi boş geçilemez.");

            RuleFor(x => x.ReturnDate)
                .NotEmpty().WithMessage("İade tarihi boş geçilemez.")
                .GreaterThan(x => x.PickupDate)
                .WithMessage("İade tarihi alış tarihinden sonra olmalıdır.");

            RuleFor(x => x.TotalPrice)
                .GreaterThan(0).WithMessage("Toplam tutar 0'dan büyük olmalıdır.");

            RuleFor(x => x.ReservationStatus)
                .NotEmpty().WithMessage("Rezervasyon durumu boş geçilemez.")
                .Must(durum => Array.IndexOf(GecerliDurumlar, durum) >= 0)
                .WithMessage("Rezervasyon durumu yalnızca Beklemede, Onaylandı veya Reddedildi olabilir.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir.");
        }
    }
}
