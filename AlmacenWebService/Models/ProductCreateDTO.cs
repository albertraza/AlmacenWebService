using AlmacenWebService.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlmacenWebService.Models
{
    public class ProductCreateDTO
    {
        [Required]
        [FirstLetterShouldBeUpperCaseAtribute]
        [StringLength(50)]
        public string Name { get; set; }
        public int CategoryId { get; set; }
        [Required]
        [Range(0.00, 99999999999999999)]
        public double Price { get; set; }
        [Required]
        [GreaterThanZeroAtribute]
        public int Quantity { get; set; }
    }
}
