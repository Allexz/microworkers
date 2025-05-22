using Microworkers.Domain.Shared;
using System.Text.RegularExpressions;

namespace Microworkers.Domain.Core.ValueObjects;

public record Phone
{
    private Phone(string number) => Number = number;
    public string Number { get; }

    public static Result<Phone> Create(string number)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(number))
            errors.Add("Phone cannot be null nor empty");

        if (!Regex.IsMatch(number,"^\\(\\d{2}\\)\\d{5}-\\d{4}$"))
            errors.Add("Phone must be in the format (XX)XXXXX-XXXX");

        if (errors.Any())
            return Result.Fail<Phone>(string.Join("; ", errors));

        return Result.Ok(new Phone(number));
    }

    // Operador de conversão implícita para long (opcional)
    public static implicit operator string(Phone phone) => phone.Number;

    // Operador de conversão explícita de long (opcional)
    public static explicit operator Phone(string number) => new(number);
}