using DataAccess.AdoNet.Interfaces;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.AdoNet
{
    public class ProductAdoNet : IProductAdoNet
    {
        private readonly string _connectionString;

        public ProductAdoNet(string connectionString)
        {
            _connectionString = connectionString;
            CreateProcedures();
        }

        public async void CreateProcedures()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string createStoredProcedureQuery = @"
                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'GetProductById') AND type in (N'P', N'PC'))
                    BEGIN
                        EXEC('
                        CREATE PROCEDURE GetProductById
                            @ProductId INT
                        AS
                        BEGIN
                            SELECT * FROM Products WHERE id = @ProductId;
                        END
                        ');
                    END";

                using (SqlCommand createStoredProcedureCommand = new SqlCommand(createStoredProcedureQuery, connection))
                {
                    await createStoredProcedureCommand.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<Product> InsertAsync(Product product)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(@"INSERT INTO Products (Name, Price, Brand, CreatedAt)
                                                 OUTPUT INSERTED.Id
                                                 VALUES  (@Name, @Price, @Brand, @CreatedAt)", connection))
                {
                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@Brand", product.Brand);
                    command.Parameters.AddWithValue("@CreatedAt", product.CreatedAt);

                    int generatedId = (int)await command.ExecuteScalarAsync();

                    if (generatedId > 0)
                    {
                        product.Id = generatedId;
                        return product;
                    }
                    else
                    {
                        throw new Exception("There was an error trying to retain the data.");
                    }
                }
            }
        }

        public async Task<Product> GetById(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Chama a Stored Procedure com um ID específico
                string getProductIdQuery = "GetProductById"; // Nome da Stored Procedure

                using (SqlCommand getProductIdCommand = new SqlCommand(getProductIdQuery, connection))
                {
                    getProductIdCommand.CommandType = CommandType.StoredProcedure;
                    getProductIdCommand.Parameters.AddWithValue("@ProductId", id);

                    using (SqlDataReader reader = await getProductIdCommand.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            Product product = new Product
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                Brand = reader.GetString(reader.GetOrdinal("Brand")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                            };

                            return product;
                        }
                        else
                        {
                            // Não há dados para ler
                            return null;
                        }
                    }
                }
            }
        }
    }
}
