using FluentValidation;
using Microworkers.Domain.Core.ValueObjects;

namespace Microworkers.Domain.Core.Validations;
public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(x => x.State)
            .NotNull()
            .NotEmpty()
            .WithMessage("State cannot be null nor empty")
            .Length(2, 2)
            .WithMessage("State must be 2 characters long");

        RuleFor(x => x.ZipCode)
            .NotNull()
            .NotEmpty()
            .WithMessage("ZipCode cannot be null nor empty")
            .Matches(@"^\d{5}-\d{3}$")
            .WithMessage("ZipCode must be in the format XXXXX-XXX");
        
        RuleFor(x => x.City)
            .NotNull()
            .NotEmpty()
            .WithMessage("City cannot be null nor empty")
            .MaximumLength(75)
            .WithMessage("City must be at most 75 characters long");

        RuleFor(x => x.NeighborHood)
            .NotNull()
            .NotEmpty()
            .WithMessage("Neighborhood cannot be null nor empty")
            .MaximumLength(30)
            .WithMessage("Neighborhood must be at most 30 characters long");

        RuleFor(x => x.Street)
            .NotNull()
            .NotEmpty()
            .WithMessage("Street cannot be null nor empty")
            .MaximumLength(75)
            .WithMessage("Street must be at most 75 characters long");

        RuleFor(x => x.Number)
            .NotNull()
            .NotEmpty()
            .WithMessage("Number cannot be null nor empty")
            .MaximumLength(10)
            .WithMessage("Number must be at most 10 characters long");

        RuleFor(x => x.Additional)
            .MaximumLength(30)
            .WithMessage("Additional must be at most 30 characters long");

    }
}
