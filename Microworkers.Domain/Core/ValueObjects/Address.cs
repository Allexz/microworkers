using Microworkers.Domain.Core.Exceptions;

namespace Microworkers.Domain.Core.ValueObjects;

public record Address
{
    public string State { get; }
    public int ZipCode { get; }
    public string City { get; }
    public string NeighborHood { get; }
    public string Street { get; }
    public string Number { get; }
    public string? Additional { get; } // Único opcional

    public Address(
        string state,
        int zipCode,
        string city,
        string neighborHood,
        string street,
        string number,
        string? additional = null)
    {
        // Validações
        State = ValidateState(state);
        ZipCode = ValidateZipCode(zipCode);
        City = ValidateCity(city);
        NeighborHood = ValidateNeighborHood(neighborHood);
        Street = ValidateStreet(street);
        Number = ValidateNumber(number);
        Additional = additional;
    }

    // Métodos de validação separados para melhor legibilidade
    private static string ValidateState(string state) =>
        state?.Length == 2
            ? state
            : throw new DomainException("State must be exactly 2 characters long", nameof(state));

    private static int ValidateZipCode(int zipCode) =>
        zipCode > 0 && zipCode.ToString().Length == 8
            ? zipCode
            : throw new DomainException("ZipCode must be a positive 8-digit number", nameof(zipCode));

    private static string ValidateCity(string city) =>
        !string.IsNullOrWhiteSpace(city) && city.Length <= 75
            ? city
            : throw new DomainException("City is required and must have up to 75 characters", nameof(city));

    private static string ValidateNeighborHood(string neighborHood) =>
        !string.IsNullOrWhiteSpace(neighborHood) && neighborHood.Length <= 75
            ? neighborHood
            : throw new DomainException("NeighborHood is required and must have up to 75 characters", nameof(neighborHood));

    private static string ValidateStreet(string street) =>
        !string.IsNullOrWhiteSpace(street) && street.Length <= 75
            ? street
            : throw new DomainException("Street is required and must have up to 75 characters", nameof(street));

    private static string ValidateNumber(string number) =>
        !string.IsNullOrWhiteSpace(number) && number.Length <= 10
            ? number
            : throw new DomainException("Number is required and must have up to 10 characters", nameof(number));
}