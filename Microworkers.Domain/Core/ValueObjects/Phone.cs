using FluentResults;

namespace Microworkers.Domain.Core.ValueObjects;

public record Phone
{
    private Phone(string number) => Number = number;
    public string Number { get; }

    public static Phone Create(string number) => new Phone(number);

    // Operador de conversão implícita para long (opcional)
    public static implicit operator string(Phone phone) => phone.Number;

    // Operador de conversão explícita de long (opcional)
    public static explicit operator Phone(string number) => new(number);
}