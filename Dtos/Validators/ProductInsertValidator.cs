﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Dtos.Validators
{
    public class ProductInsertValidator
    {
        public static string ValidateProduct(ProductInsertDto productInsertDto)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(productInsertDto, serviceProvider: null, items: null);

            bool isNotValid = !Validator.TryValidateObject(productInsertDto, validationContext, validationResults, validateAllProperties: true);

            if (isNotValid)
            {
                return validationResults.FirstOrDefault().ToString();
            }

            return null;
        }
    }
}
