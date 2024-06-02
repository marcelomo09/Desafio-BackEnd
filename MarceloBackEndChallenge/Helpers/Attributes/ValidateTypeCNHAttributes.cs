using System.ComponentModel.DataAnnotations;

public class ValidateTypeCNHAttribute : ValidationAttribute
{
    public ValidateTypeCNHAttribute()
    {

    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var validValues = new [] { "A", "B", "A+B" };

        if (value != null && !Array.Exists(validValues, e => e == value.ToString()))
        {
            return new ValidationResult("Apenas carteiras A e B ou ambas A+B são validas");
        }

        return ValidationResult.Success;
    }
}