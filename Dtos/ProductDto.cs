using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
    public class ProductDto
    {
        [Required(ErrorMessage = "The name cannot be empty")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The price cannot be empty")]
        [Range(1, double.MaxValue, ErrorMessage = "The price cannot have a value less than or equal to zero")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "The brand cannot be empty")]
        public string Brand { get; set; }
    }
}
