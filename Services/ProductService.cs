using Dtos;
using Dtos.Validators;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using Services.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class ProductService : IProductService
    {
        public async Task<Product> PostProduct(ProductDto productDto)
        {
            string errorMessage = ProductInsertValidator.ValidateProduct(productDto);

            if (errorMessage != null)
                throw new ValidationException($"Erro de validação: {errorMessage}");

            // Continue com a lógica do serviço
            return null;
        }
    }
}
