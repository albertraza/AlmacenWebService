using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlmacenWebService.Entities.Abstactions
{
    public abstract class DatabaseHandler<T> : ICrud<T>
    {
        public abstract Task<T> CreateAsync(T obj);

        public abstract Task DeleteAsync(int id);

        public abstract Task<IEnumerable<T>> GetAllAsync();

        public abstract Task<T> GetAsync(int id);

        public abstract Task UpdateAsync(T obj);
    }
}
