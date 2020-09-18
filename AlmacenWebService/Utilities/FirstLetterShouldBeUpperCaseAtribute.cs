using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlmacenWebService.Utilities
{
    public class FirstLetterShouldBeUpperCaseAtribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            var firstLetter = value.ToString()[0].ToString();

            if (firstLetter != firstLetter.ToUpper())
                return new ValidationResult("La primera letra debe ser mayuscula");

            return ValidationResult.Success;
        }
    }
}
