﻿using Dtos;
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

        public async Task<Product> PostProduct(ProductDto productDto)
        {
            string errorMessage = ProductInsertValidator.ValidateProduct(productDto);

            if (errorMessage != null)
                throw new ValidationException($"Erro de validação: {errorMessage}");

            var productAdonet = new ProductAdoNet(_connectionString);

            var product = await productAdonet.InsertAsync(productDto);

            return product;
        }
    }
}