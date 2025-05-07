namespace Microworkers.Domain.Core.ValueObjects;

public readonly record struct Phone
{
    public long Number { get; }

    public Phone(long number)
    {
        if (number <= 0)
            throw new ArgumentException("Phone number must be positive.", nameof(number));

        if (number.ToString().Length != 9)
            throw new ArgumentException("Phone number must have at 9 digits.", nameof(number));

        Number = number;
    }

    // Operador de conversão implícita para long (opcional)
    public static implicit operator long(Phone phone) => phone.Number;

    // Operador de conversão explícita de long (opcional)
    public static explicit operator Phone(long number) => new(number);
}