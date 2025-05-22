using FluentValidation;
using Microworkers.Domain.Core.ValueObjects;

namespace Microworkers.Domain.Core.Validations;
public class PhoneValidator : AbstractValidator<Phone>
{
    public PhoneValidator()
    {
        RuleFor(x => x.Number)
            .NotNull()
            .NotEmpty()
            .WithMessage("Phone cannot be null nor empty")
            .Matches("^\\(\\d{2}\\)\\d{5}-\\d{4}$")
            .WithMessage("Phone must be in the format (XX)XXXXX-XXXX");
    }
}
