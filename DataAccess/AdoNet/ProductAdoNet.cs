using DataAccess.AdoNet.Interfaces;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
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
    }
}
