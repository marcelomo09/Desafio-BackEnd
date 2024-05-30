using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

public class ValidDateAttribute : ValidationAttribute
{
    private readonly string _dateFormat;

    public ValidDateAttribute(string dateFormat)
    {
        _dateFormat = dateFormat;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return new ValidationResult("O campo data é obrigatório.");
        }

        DateTime date;
        bool isValid = DateTime.TryParseExact(value.ToString(), _dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

        if (!isValid)
        {
            return new ValidationResult($"A data deve estar no formato {_dateFormat}.");
        }

        return  ValidationResult.Success;
    }
}
