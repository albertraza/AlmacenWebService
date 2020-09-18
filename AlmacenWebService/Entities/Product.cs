using AlmacenWebService.Entities.Abstactions;
using AlmacenWebService.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlmacenWebService.Entities
{
    public class Product: IProduct
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


        public Product()
        {

        }

        public Product(string name)
        {
            Name = name;
        }

        public Product(string name, int categoryId)
        {
            Name = name;
            CategoryId = categoryId;
        }

        public Product(string name, int categoryId, double price)
        {
            Name = name;
            CategoryId = categoryId;
            Price = price;
        }
    }
}
