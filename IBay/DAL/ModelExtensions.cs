using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DAL.Model;

namespace DAL
{
    public static class ModelExtensions
    {
        public static string ValidateUser(this User user)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(user, new ValidationContext(user), validationResults, true))
            {
                string errorMessage = string.Join(Environment.NewLine, validationResults.Select(x => x.ErrorMessage));
                return errorMessage;
            }
            return string.Empty;
        }

        public static string ValidateProduct(this Product product)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(product, new ValidationContext(product), validationResults, true))
            {
                string errorMessage = string.Join(Environment.NewLine, validationResults.Select(x => x.ErrorMessage));
                return errorMessage;
            }
            return string.Empty;
        }

        public static string ValidateCart(this Cart cart)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(cart, new ValidationContext(cart), validationResults, true))
            {
                string errorMessage = string.Join(Environment.NewLine, validationResults.Select(x => x.ErrorMessage));
                return errorMessage;
            }
            return string.Empty;
        }
    }
}
