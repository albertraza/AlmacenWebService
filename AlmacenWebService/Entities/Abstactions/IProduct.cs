using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlmacenWebService.Entities.Abstactions
{
    public interface IProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public double Price { get; set; }

    }
}
