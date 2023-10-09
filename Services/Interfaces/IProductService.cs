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
        Task<Product> PostProductAsync(ProductInsertDto productDto);
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> UpdateProductAsync(ProductUpdateDto productDto, int id);
    }
}
