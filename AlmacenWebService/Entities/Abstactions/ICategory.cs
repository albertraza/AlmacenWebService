using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlmacenWebService.Entities.Abstactions
{
    public interface ICategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


    public interface ICrud<T>
    {
        Task<T> CreateAsync(T obj);
        Task UpdateAsync(T obj);
        Task DeleteAsync(int id);
        Task<T> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
    }
}
