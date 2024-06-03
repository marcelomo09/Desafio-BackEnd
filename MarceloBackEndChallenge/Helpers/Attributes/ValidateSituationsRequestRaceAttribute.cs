using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

public class ValidateSituationsRequestRaceAttribute : ValidationAttribute
{
    public ValidateSituationsRequestRaceAttribute()
    {   
        
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var serviceProvider = validationContext.GetRequiredService<IServiceProvider>();

        var mongoSettings  = serviceProvider.GetRequiredService<IOptions<MongoSettings>>().Value;

        var situations = new [] { mongoSettings.Collections.RequestRides.Situations.Disponible, 
                                  mongoSettings.Collections.RequestRides.Situations.Accept, 
                                  mongoSettings.Collections.RequestRides.Situations.Deliver };

        if (value != null && !Array.Exists(situations, e => e == value.ToString()))
        {
            return new ValidationResult($"As situações disponiveis são {string.Join(", ", mongoSettings.Collections.RequestRides.Situations)}");
        }

        return ValidationResult.Success;
    }
}