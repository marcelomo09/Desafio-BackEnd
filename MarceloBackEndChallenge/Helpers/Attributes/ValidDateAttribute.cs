using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

public class ValidDateAttribute : ValidationAttribute
{
    private readonly string _dateFormat;

    private readonly bool _dateMoreThenNow;

    public ValidDateAttribute(string dateFormat, bool dateMoreThenNow = false)
    {
        _dateFormat      = dateFormat;
        _dateMoreThenNow = dateMoreThenNow;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return new ValidationResult("O campo data é obrigatório.");
        }

        DateTime date;
        bool isValid = DateTime.TryParseExact(value.ToString(), _dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

        if (!isValid) return new ValidationResult($"A data deve estar no formato {_dateFormat}.");

        if (_dateMoreThenNow && date <= DateTime.Now) return new ValidationResult(ErrorMessage ?? "A data deve ser maior que a data atual.");

        return  ValidationResult.Success;
    }
}
