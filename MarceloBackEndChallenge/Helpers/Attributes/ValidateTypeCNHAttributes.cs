using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

public class ValidateTypeCNHAttribute : ValidationAttribute
{
    public ValidateTypeCNHAttribute()
    {

    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var serviceProvider = validationContext.GetRequiredService<IServiceProvider>();

        var mongoSettings  = serviceProvider.GetRequiredService<IOptions<MongoSettings>>().Value;

        var typesCNH = mongoSettings.Collections.DeliveryDrivers.TypesCNH;

        if (value != null && !typesCNH.Any(a => a == value.ToString()))
        {
            return new ValidationResult($"Tipo de carteira inválida, as permitidas são {string.Join(", ", typesCNH)}");
        }

        return ValidationResult.Success;
    }
}