using FluentResults;

namespace Microworkers.Domain.Core.ValueObjects;

public readonly record struct Phone
{
    private Phone(long number) => Number = number;
    public long Number { get; }

    public static Result<Phone> Create(long number)
    {
        List<Error> errors = new();
        if (number <= 0)
           errors
                .Add(new Error("Phone number must be positive.")
                .WithMetadata("Field", nameof(number)));

        string numberToString = number.ToString();
        if (numberToString.Length != 9)
            errors
                .Add(new Error("Phone number must have at 9 digits.")
                .WithMetadata("Field", nameof(number)));
        if (errors.Any())
            return Result.Fail<Phone>(errors);

        return Result.Ok(new Phone(number));
    }

    // Operador de conversão implícita para long (opcional)
    public static implicit operator long(Phone phone) => phone.Number;

    // Operador de conversão explícita de long (opcional)
    public static explicit operator Phone(long number) => new(number);
}