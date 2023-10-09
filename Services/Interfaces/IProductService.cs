using DataAccess.Entities;
using Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IProductService
    {
        Task<Product> PostProduct(ProductDto productDto);
        Task<Product> GetProductById(int id);
        Task<IEnumerable<Product>> GetProducts();
    }
}
