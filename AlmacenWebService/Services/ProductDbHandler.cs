﻿using AlmacenWebService.Entities;
using AlmacenWebService.Entities.Abstactions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AlmacenWebService.Services
{
    public class ProductDbHandler : DatabaseHandler<Product>
    {
        private readonly IConfiguration configuration;

        public ProductDbHandler(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public override Task<int> Count()
        {
            throw new NotImplementedException();
        }

        public override async Task<Product> CreateAsync(Product obj)
        {
            using var connection = new SqlConnection(configuration.GetConnectionString("defaultConnectionString"));
            using var command = new SqlCommand();

            await connection.OpenAsync();

            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "createProduct";
            command.Connection = connection;

            command.Parameters.Add(new SqlParameter("@name", System.Data.SqlDbType.VarChar)).Value = obj.Name;
            command.Parameters.Add(new SqlParameter("@category", System.Data.SqlDbType.Int)).Value = obj.CategoryId;
            command.Parameters.Add(new SqlParameter("@price", System.Data.SqlDbType.Float)).Value = obj.Price;
            command.Parameters.Add(new SqlParameter("@quantity", System.Data.SqlDbType.Int)).Value = obj.Quantity;

            var productId = (int)await command.ExecuteScalarAsync();
            obj.Id = productId;

            await connection.CloseAsync();

            return obj;
        }

        public override async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(configuration.GetConnectionString("defaultConnectionString"));
            using var command = new SqlCommand();

            await connection.OpenAsync();

            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "deleteProduct";
            command.Connection = connection;

            command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = id;

            await connection.CloseAsync();

            await command.ExecuteNonQueryAsync();
        }

        public override async Task<IEnumerable<Product>> GetAllAsync()
        {
            using var connection = new SqlConnection(configuration.GetConnectionString("defaultConnectionString"));
            using var command = new SqlCommand();

            await connection.OpenAsync();

            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "getProducts";
            command.Connection = connection;


            using var reader = await command.ExecuteReaderAsync();
            var productList = new List<Product>();

            while (await reader.ReadAsync())
            {
                productList.Add(MapToEntity(reader));
            }

            await connection.CloseAsync();

            return productList;
        }

        public override async Task<Product> GetAsync(int id)
        {
            using var connection = new SqlConnection(configuration.GetConnectionString("defaultConnectionString"));
            using var command = new SqlCommand();

            await connection.OpenAsync();

            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "getProduct";
            command.Connection = connection;

            command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = id;

            using var reader = await command.ExecuteReaderAsync();
            var product = new Product();

            while (await reader.ReadAsync())
            {
                product = MapToEntity(reader);
            }

            return product;
        }

        public override async Task UpdateAsync(Product obj)
        {
            using var connection = new SqlConnection(configuration.GetConnectionString("defaultConnectionString"));
            using var command = new SqlCommand();

            await connection.OpenAsync();

            command.CommandText = "updateProduct";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Connection = connection;

            command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = obj.Id;
            command.Parameters.Add(new SqlParameter("@name", System.Data.SqlDbType.VarChar)).Value = obj.Name;
            command.Parameters.Add(new SqlParameter("@categoryId", System.Data.SqlDbType.Int)).Value = obj.CategoryId;
            command.Parameters.Add(new SqlParameter("@price", System.Data.SqlDbType.Float)).Value = obj.Price;

            await command.ExecuteNonQueryAsync();

            await connection.CloseAsync();
        }

        private Product MapToEntity(SqlDataReader reader)
        {
            return new Product(
                                Convert.ToInt32(reader["id"].ToString()),
                                reader["name"].ToString(),
                                Convert.ToInt32(reader["categoryId"].ToString()),
                                Convert.ToDouble(reader["price"].ToString()),
                                Convert.ToInt32(reader["quantity"].ToString())
                                );

        }
    }
}
