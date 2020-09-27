using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlmacenWebService.Utilities
{
    public class GreaterThanZeroAtribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value.GetType() == typeof(int))
                return new ValidationResult("Se requiere que el valor sea un numero");

            var intValue = (int)value;

            if (intValue <= 0)
                return new ValidationResult("El valor debe ser mayor que 0");

            return ValidationResult.Success;
        }
    }
}
