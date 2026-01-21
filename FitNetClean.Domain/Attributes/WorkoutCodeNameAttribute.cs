using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FitNetClean.Domain.Attributes;

/// <summary>
/// Custom validation attribute for workout code names.
/// Rules:
/// - Length: 6-12 characters
/// - Only uppercase letters (A-Z), numbers (0-9), underscore (_), and exclamation mark (!)
/// - Cannot start with a number
/// - Cannot contain whitespace
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class WorkoutCodeNameAttribute : ValidationAttribute
{
    private const int MinLength = 6;
    private const int MaxLength = 12;
    private const string AllowedCharactersPattern = @"^[A-Z0-9_!]+$";

    public WorkoutCodeNameAttribute()
        : base("The CodeName must be 6-12 characters long, contain only uppercase letters, numbers, underscore (_) and exclamation mark (!), and cannot start with a number.")
    {
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            return new ValidationResult("CodeName is required.");
        }

        if (value is not string codeName)
        {
            return new ValidationResult("CodeName must be a string.");
        }

        if (codeName.Length < MinLength || codeName.Length > MaxLength)
        {
            return new ValidationResult($"CodeName must be between {MinLength} and {MaxLength} characters long.");
        }

        if (char.IsDigit(codeName[0]))
        {
            return new ValidationResult("CodeName cannot start with a number.");
        }

        if (codeName.Contains(' ') || codeName.Any(char.IsWhiteSpace))
        {
            return new ValidationResult("CodeName cannot contain whitespace.");
        }

        if (!Regex.IsMatch(codeName, AllowedCharactersPattern))
        {
            return new ValidationResult("CodeName can only contain uppercase letters (A-Z), numbers (0-9), underscore (_), and exclamation mark (!).");
        }

        return ValidationResult.Success;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"The {name} field is invalid. It must be {MinLength}-{MaxLength} characters long, " +
               "contain only uppercase letters, numbers, underscore (_) and exclamation mark (!), " +
               "and cannot start with a number.";
    }
}
