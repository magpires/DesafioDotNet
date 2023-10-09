using Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Product : BaseEntity
    {
        public Product() { }
        
        public Product(string name, decimal price, string brand)
        {
            Name = name;
            Price = price;
            Brand = brand;
        }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Brand { get; set; }

        public static implicit operator Product(ProductDto productDto)
        {
            return new Product(productDto.Name, productDto.Price, productDto.Brand);
        }
    }
}
