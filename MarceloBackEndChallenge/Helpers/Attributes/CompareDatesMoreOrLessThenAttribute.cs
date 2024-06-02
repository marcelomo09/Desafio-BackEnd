using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

public class CompareDatesMoreOrLessThenAttribute : ValidationAttribute
{
    private readonly string _comparisonProperty;

    public CompareDatesMoreOrLessThenAttribute(string comparisonProperty)
    {
        _comparisonProperty = comparisonProperty;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("O campo data é obrigatório.");
        }

        DateTime currentValue;

        if (value is string strValue)
        {
            if (!DateTime.TryParse(strValue, out currentValue))
            {
                return new ValidationResult("Formato de data inválido.");
            }
        }
        else if (value is DateTime dtValue)
        {
            currentValue = dtValue;
        }
        else
        {
            return new ValidationResult("Formato de data inválido.");
        }

        var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
        if (property == null)
        {
            return new ValidationResult($"Propriedade {_comparisonProperty} não encontrada.");
        }

        var comparisonValue = property.GetValue(validationContext.ObjectInstance);

        DateTime comparisonDate;

        if (comparisonValue is string comparisonStr)
        {
            if (!DateTime.TryParse(comparisonStr, out comparisonDate))
            {
                return new ValidationResult("Formato de data inválido.");
            }
        }
        else if (comparisonValue is DateTime comparisonDt)
        {
            comparisonDate = comparisonDt;
        }
        else
        {
            return new ValidationResult("Formato de data inválido.");
        }

        if (currentValue < comparisonDate)
        {
            return new ValidationResult(ErrorMessage ?? "A data de término deve ser maior ou igual à data de início.");
        }

        return ValidationResult.Success;
    }
}