using System;

namespace Carola.BusinessLayer.ValidationRules
{
    public static class CustomerAgeRule
    {
        public static bool BeAtLeast18(DateTime birthDate)
        {
            if (birthDate == default) return false;

            var age = DateTime.Today.Year - birthDate.Year;
            if (birthDate.Date > DateTime.Today.AddYears(-age)) age--;

            return age >= 18;
        }
    }
}
