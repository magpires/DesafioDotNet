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

                using (SqlCommand command = new SqlCommand($@"INSERT INTO Products (Name, Price, Brand, CreatedAt)
                                                                    OUTPUT INSERTED.Id
                                                                    VALUES  ('{product.Name}', {product.Price}, '{product.Brand}','{product.CreatedAt}')", connection))
                {

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
