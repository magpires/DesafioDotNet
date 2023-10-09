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

        public void CreateProcedures()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.OpenAsync();

                string createStoredProcedureQueryGetProductById = @"
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

                using (SqlCommand createStoredProcedureCommand = new SqlCommand(createStoredProcedureQueryGetProductById, connection))
                {
                    createStoredProcedureCommand.ExecuteNonQueryAsync();
                }

                string createStoredProcedureQueryGetProducts = @"
                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'GetProducts') AND type in (N'P', N'PC'))
                    BEGIN
                        EXEC('
                        CREATE PROCEDURE GetProducts
                        AS
                        BEGIN
                            SELECT * FROM Products;
                        END
                        ');
                    END";

                using (SqlCommand createStoredProcedureCommand = new SqlCommand(createStoredProcedureQueryGetProducts, connection))
                {
                    createStoredProcedureCommand.ExecuteNonQueryAsync();
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

        public async Task<Product> GetByIdAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string getProductIdQuery = "GetProductById";

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
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                            };

                            return product;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string getProductsQuery = "GetProducts";
                var products = new List<Product>();

                using (SqlCommand getProductsCommand = new SqlCommand(getProductsQuery, connection))
                {
                    getProductsCommand.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = await getProductsCommand.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Product product = new Product
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                Brand = reader.GetString(reader.GetOrdinal("Brand")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                            };

                            products.Add(product);
                        }
                    }
                }

                return products;
            }
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(@"
                                                    UPDATE Products
                                                    SET
                                                        Name = @Name,
                                                        Price = @Price,
                                                        Brand = @Brand,
                                                        UpdatedAt = @UpdatedAt
                                                    WHERE
                                                        Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@Brand", product.Brand);
                    command.Parameters.AddWithValue("@UpdatedAt", product.UpdatedAt);
                    command.Parameters.AddWithValue("@Id", product.Id);

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        return product;
                    }
                    else
                    {
                        throw new Exception("An error occurred while trying to update the data.");
                    }
                }
            }
        }
    }
}
