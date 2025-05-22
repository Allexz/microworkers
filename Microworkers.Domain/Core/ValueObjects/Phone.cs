using FluentResults;
using Microworkers.Domain.Core.Exceptions;
using System.Text.RegularExpressions;

namespace Microworkers.Domain.Core.ValueObjects;

public record Phone
{
    private Phone(string number) => Number = number;
    public string Number { get; }

    public static Phone Create(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new InvalidPhoneException("Phone cannot be null nor empty", nameof(number));

        if (!Regex.IsMatch(number,"^\\(\\d{2}\\)\\d{5}-\\d{4}$"))
            throw new InvalidPhoneException("Phone must be in the format (XX)XXXXX-XXXX", nameof(number));

        return new Phone(number);
    }

    // Operador de conversão implícita para long (opcional)
    public static implicit operator string(Phone phone) => phone.Number;

    // Operador de conversão explícita de long (opcional)
    public static explicit operator Phone(string number) => new(number);
}