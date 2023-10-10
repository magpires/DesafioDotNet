using Dtos;
using Dtos.Validators;
using Services.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.AdoNet;
using System;
using DataAccess.Entities;
using System.Configuration;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly string _connectionString;

        public ProductService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public async Task<Product> PostProductAsync(ProductInsertDto productInsertDto)
        {
            string errorMessage = ProductInsertValidator.ValidateProduct(productInsertDto);

            if (errorMessage != null)
                throw new ValidationException($"Error validation: {errorMessage}");

            var productAdonet = new ProductAdoNet(_connectionString);

            var product = await productAdonet.InsertAsync(productInsertDto);

            return product;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var productAdonet = new ProductAdoNet(_connectionString);
            var product = await productAdonet.GetByIdAsync(id);

            if (product == null)
                throw new ValidationException($"This product not found.");

            return product;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            var productAdonet = new ProductAdoNet(_connectionString);
            var products = await productAdonet.GetAllAsync();

            return products;
        }

        public void MergeProperties(ProductUpdateDto source, Product destiny)
        {
            destiny.Name = string.IsNullOrEmpty(source.Name) ? destiny.Name : source.Name;
            destiny.Price = source.Price.HasValue && source.Price > 0 ? source.Price.Value : destiny.Price;
            destiny.Brand = string.IsNullOrEmpty(source.Brand) ? destiny.Brand : source.Brand;
        }

        public async Task<Product> UpdateProductAsync(ProductUpdateDto productUpdateDto, int id)
        {
            string errorMessage = ProductUpdateValidator.ValidateProduct(productUpdateDto);

            if (errorMessage != null)
                throw new ValidationException($"Error validation: {errorMessage}");

            var productAdonet = new ProductAdoNet(_connectionString);

            var productDatabase = await productAdonet.GetByIdAsync(id);

            if (productDatabase == null)
                throw new ValidationException($"This product not found.");

            MergeProperties(productUpdateDto, productDatabase);
            productDatabase.AlterUpdatedAt();
            await productAdonet.UpdateAsync(productDatabase);

            return productDatabase;
        }

        public async Task<bool> RemoveProductAsync(int id)
        {
            var productAdonet = new ProductAdoNet(_connectionString);
            var productNotFound = await productAdonet.GetByIdAsync(id) == null;

            if (productNotFound)
                throw new ValidationException($"This product not found.");

            return await productAdonet.DeleteAsync(id);
        }
    }
}
