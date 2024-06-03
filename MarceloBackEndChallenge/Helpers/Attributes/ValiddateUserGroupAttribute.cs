using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

public class ValiddateUserGroupAttribute: ValidationAttribute
{
    public ValiddateUserGroupAttribute()
    {
        
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var serviceProvider = validationContext.GetRequiredService<IServiceProvider>();

        var mongoSettings  = serviceProvider.GetRequiredService<IOptions<MongoSettings>>().Value;

        var userGroups = mongoSettings.Collections.Users.UserGroups;

        if (value != null && !userGroups.Any(a => a == value.ToString()))
        {
            return new ValidationResult($"Grupo de usuário inválido, os grupos disponiveis são {string.Join(", ", userGroups)}");
        }

        return ValidationResult.Success;
    }
}