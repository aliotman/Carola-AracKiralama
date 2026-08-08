using Carola.EntityLayer.Entities;
using FluentValidation;

namespace Carola.BusinessLayer.ValidationRules
{
    public class SliderValidator : AbstractValidator<Slider>
    {
        public SliderValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık boş geçilemez.")
                .Length(3, 100).WithMessage("Başlık 3 ile 100 karakter arasında olmalıdır.");

            RuleFor(x => x.Subtitle)
                .MaximumLength(150).WithMessage("Alt başlık en fazla 150 karakter olabilir.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir.");

            RuleFor(x => x.BackgroundImageUrl)
                .NotEmpty().WithMessage("Arka plan görseli boş geçilemez.")
                .Must(ImageUrlRule.BeAValidImageUrl)
                .WithMessage("Arka plan görseli geçerli bir http/https adresi olmalıdır.");

            RuleFor(x => x.Order)
                .GreaterThanOrEqualTo(0).WithMessage("Sıra değeri negatif olamaz.");
        }
    }
}
