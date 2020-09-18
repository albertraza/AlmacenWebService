using AlmacenWebService.Entities.Abstactions;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace AlmacenWebService.Entities
{
    public class CategoryDbHandler : DatabaseHandler<Category>
    {
        private readonly IConfiguration configuration;

        public CategoryDbHandler(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public override async Task<Category> CreateAsync(Category obj)
        {
            using var connection = new SqlConnection(configuration.GetConnectionString("defaultConnectionString"));
            using var command = new SqlCommand();
            await connection.OpenAsync();

            command.Connection = connection;
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "createCategory";

            command.Parameters.Add(new SqlParameter("@name", System.Data.SqlDbType.VarChar)).Value = obj.Name;

            var categoryId = (int)(await command.ExecuteScalarAsync());

            obj.Id = categoryId;
            await connection.CloseAsync();
            return obj;
        }

        public override async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(configuration.GetConnectionString("defaultConnectionString"));
            using var command = new SqlCommand();

            await connection.OpenAsync();

            command.Connection = connection;
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "deleteCategory";

            command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = id;

            await connection.CloseAsync();

            await command.ExecuteNonQueryAsync();
        }

        public override async Task<IEnumerable<Category>> GetAllAsync()
        {
            using var connection = new SqlConnection(configuration.GetConnectionString("defaultConnectionString"));
            using var command = new SqlCommand();

            await connection.OpenAsync();

            command.Connection = connection;
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "getCategories";

            var reader = await command.ExecuteReaderAsync();
            var categoryList = new List<Category>();

            while (reader.Read())
            {
                categoryList.Add(new Category(Convert.ToInt32(reader["id"].ToString()), reader["name"].ToString()));
            }

            await connection.CloseAsync();

            return categoryList;
        }

        public override async Task<Category> GetAsync(int id)
        {
            using var connection = new SqlConnection(configuration.GetConnectionString("defaultConnectionString"));
            using var command = new SqlCommand();

            await connection.OpenAsync();

            command.Connection = connection;
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "getCategory";

            command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = id;

            var reader = await command.ExecuteReaderAsync();
            var category = new Category();

            while (reader.Read())
            {
                category.Id = Convert.ToInt32(reader["id"].ToString());
                category.Name = reader["name"].ToString();
            }

            await connection.CloseAsync();

            return category;
        }

        public override async Task UpdateAsync(Category obj)
        {
            using var connection = new SqlConnection(configuration.GetConnectionString("defaultConnectionString"));
            using var command = new SqlCommand();

            await connection.OpenAsync();

            command.Connection = connection;
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "updateCategory";

            command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = obj.Id;
            command.Parameters.Add(new SqlParameter("@name", System.Data.SqlDbType.VarChar)).Value = obj.Name;

            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();

        }
    }
}
