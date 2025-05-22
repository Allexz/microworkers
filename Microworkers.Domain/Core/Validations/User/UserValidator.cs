using FluentValidation;
using Do = Microworkers.Domain.Core.Aggregates;

namespace Microworkers.Domain.Core.Validations;
public class UserValidator : AbstractValidator<Do.User>
{
    public UserValidator()
    {
        RuleFor(user => user.Name)
            .NotEmpty()
            .WithMessage("Name cannot be empty.")
            .Length(3,75)
            .WithMessage("Name must be at least 3 character and cannot be longer than 75.");

        RuleFor(user => user.Document)
            .NotEmpty()
            .WithMessage("Document cannot be empty.")
            .Matches(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$")
            .WithMessage("Document must be in the format XXX.XXX.XXX-XX.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password cannot be empty.")
            .Length(10,100)
            .WithMessage("Password must be at least 10 character and cannot be longer than 100");

        RuleFor(x => x.Phone).SetValidator(new PhoneValidator());

        RuleFor(x => x.Username)
            .NotNull()
            .NotEmpty()
            .WithMessage("Username cannot be null nor empty")
            .EmailAddress()
            .WithMessage("Username must be a valid email address");

        RuleFor(x => x.Address).SetValidator(new AddressValidator());
    }
}
