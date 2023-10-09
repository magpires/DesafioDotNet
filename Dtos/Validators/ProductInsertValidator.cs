using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Dtos.Validators
{
    public class ProductInsertValidator
    {
        public static string ValidateProduct(ProductDto productDto)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(productDto, serviceProvider: null, items: null);

            List<string> errorMessages = new List<string>();

            bool isNotValid = !Validator.TryValidateObject(productDto, validationContext, validationResults, validateAllProperties: true);

            if (isNotValid)
            {
                return validationResults.FirstOrDefault().ToString();
            }

            return null;
        }
    }
}
