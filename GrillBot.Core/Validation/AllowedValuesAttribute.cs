using System.ComponentModel.DataAnnotations;

namespace GrillBot.Core.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public class AllowedValuesAttribute : ValidationAttribute
{
    private object[] Values { get; }

    public AllowedValuesAttribute(params object[] values)
    {
        Values = values;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
            return ValidationResult.Success;

        if (!Array.Exists(Values, o => o == value))
            return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName! });
        return ValidationResult.Success;
    }
}
