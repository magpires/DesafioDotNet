using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Dtos.Validators
{
    public class ProductUpdateValidator
    {
        public static string ValidateProduct(ProductUpdateDto productUpdateDto)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(productUpdateDto, serviceProvider: null, items: null);

            bool isNotValid = !Validator.TryValidateObject(productUpdateDto, validationContext, validationResults, validateAllProperties: true);

            if (isNotValid)
            {
                return validationResults.FirstOrDefault().ToString();
            }

            return null;
        }
    }
}
