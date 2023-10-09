using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
    public class ProductUpdateDto
    {
        public string Name { get; set; }
        [Range(1, double.MaxValue, ErrorMessage = "The price cannot have a value less than or equal to zero")]
        public decimal? Price { get; set; }
        public string Brand { get; set; }
    }
}
