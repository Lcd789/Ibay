using System.ComponentModel.DataAnnotations;
using DAL.Model;

namespace DAL
{
    public static class ModelExtensions
    {
        public static string ValidateUser(this User user)
        {
            var validationResults = new List<ValidationResult>();
            if (Validator.TryValidateObject(user, new ValidationContext(user), validationResults, true))
                return string.Empty;
            var errorMessage = string.Join(Environment.NewLine, validationResults.Select(x => x.ErrorMessage));
            return errorMessage;
        }

        public static string ValidateProduct(this Product product)
        {
            var validationResults = new List<ValidationResult>();
            if (Validator.TryValidateObject(product, new ValidationContext(product), validationResults, true))
                return string.Empty;
            var errorMessage = string.Join(Environment.NewLine, validationResults.Select(x => x.ErrorMessage));
            return errorMessage;
        }
    }
}
