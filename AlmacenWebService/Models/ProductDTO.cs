using AlmacenWebService.Utilities;
using System;
using System.ComponentModel.DataAnnotations;


namespace AlmacenWebService.Models
{
    public class ProductDTO
    {
        public int Id { get; set; }

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
