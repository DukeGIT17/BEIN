using System.ComponentModel.DataAnnotations;

namespace BEIN_DL.CustomAttributes
{
    public class SkipValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            return ValidationResult.Success;
        }
    }
}
