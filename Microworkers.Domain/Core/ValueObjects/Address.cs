using FluentResults;

namespace Microworkers.Domain.Core.ValueObjects;

public sealed record Address
{
    private Address() { }

    public string State { get; init; }
    public int ZipCode { get; init; }
    public string City { get; init; }
    public string NeighborHood { get; init; }
    public string Street { get; init; }
    public string Number { get; init; }
    public string? Additional { get; init; } // Único opcional

    public Address With(
    string? state = null,
    int? zipCode = null,
    string? city = null,
    string? neighborHood = null,
    string? street = null,
    string? number = null,
    string? additional = null)
    {
        return new Address
        {
            State = state ?? this.State,
            ZipCode = zipCode ?? this.ZipCode,
            City = city ?? this.City,
            NeighborHood = neighborHood ?? this.NeighborHood,
            Street = street ?? this.Street,
            Number = number ?? this.Number,
            Additional = additional ?? this.Additional
        };
    }

    public static Result<Address> Create(
        string state,
        int zipCode,
        string city,
        string neighborHood,
        string street,
        string number,
        string? additional = null)
    {
        List<Error> errors = new();
        // Validações
        ValidateState(state, errors);
        ValidateZipCode(zipCode, errors);
        ValidateCity(city, errors);
        ValidateNeighborHood(neighborHood, errors);
        ValidateStreet(street, errors);
        ValidateNumber(number, errors);
        ValidateAdditional(additional, errors);

        if (errors.Any())
            return Result.Fail<Address>(errors);

        return Result.Ok(new Address
        {
            State = state,
            ZipCode = zipCode,
            City = city,
            NeighborHood = neighborHood,
            Street = street,
            Number = number,
            Additional = additional
        });
    }

    public static Result<Address> Update(
        Address original,
        string? pState = null,
        int? pZipCode = null,
        string? pCity = null,
        string? pNeighborHood = null,
        string? pStreet = null,
        string? pNumber = null,
        string? pAdditional = null)
    {
        List<Error> errors = new();
        // Validações
        if(pState != null ) ValidateState(pState, errors);
        if(pZipCode.HasValue) ValidateZipCode(pZipCode.Value, errors);
        if(pCity != null) ValidateCity(pCity, errors);
        if(pNeighborHood != null) ValidateNeighborHood(pNeighborHood, errors);
        if(pStreet != null) ValidateStreet(pStreet, errors);
        if(pNumber != null) ValidateNumber(pNumber, errors);
        if(pAdditional != null) ValidateAdditional(pAdditional, errors);

        if (errors.Any())
            return Result.Fail<Address>(errors);

        return Result.Ok(new Address
        {
            State = pState ?? original.State ,
            ZipCode = pZipCode.HasValue ? pZipCode.Value : original.ZipCode,
            City = pCity ?? original.City,
            NeighborHood = pNeighborHood ?? original.NeighborHood,
            Street = pState ?? original.Street,
            Number = pNumber ?? original.Number,
            Additional = pAdditional ?? original.Additional
        });
    }

    private static void ValidateAdditional(string? additional, List<Error> errors)
    {
        if (additional?.Length > 30)
        {
            errors.Add(new Error("Additional is required and must have up to 30 characters")
                .WithMetadata("Field", nameof(additional)));
        }
    }

    // Métodos de validação separados para melhor legibilidade
    private static void ValidateState(string state, List<Error> errors)
    {
        if (string.IsNullOrWhiteSpace(state) || state.Length != 2)
        {
             errors.Add(new Error("State is required and must have exactly 2 characters")
                .WithMetadata("Field", nameof(state)));
        }
    }

    private static void ValidateZipCode(int zipCode, List<Error> errors)
    {
        if (zipCode <= 0)
        {
             errors.Add(new Error("ZipCode must be a positive number")
                .WithMetadata("Field", nameof(zipCode)));
        }
        else if (zipCode.ToString().Length != 8)
        {
             errors.Add(new Error("ZipCode must have exactly 8 digits")
                .WithMetadata("Field", nameof(zipCode)));
        }
    }

    private static void ValidateCity(string city, List<Error> errors)
    {
        if (string.IsNullOrWhiteSpace(city) || city.Length > 75)
        {
             errors.Add(new Error("City is required and must have up to 75 characters")
                .WithMetadata("Field", nameof(city)));
        }
    }

    private static void ValidateNeighborHood(string neighborHood, List<Error> errors)
    {
        if (string.IsNullOrWhiteSpace(neighborHood) || neighborHood.Length > 75)
        {
             errors.Add(new Error("NeighborHood is required and must have up to 75 characters")
                .WithMetadata("Field", nameof(neighborHood)));
        }
    }
    private static void ValidateStreet(string street, List<Error> errors)
    {
        if (string.IsNullOrWhiteSpace(street) || street.Length > 75)
        {
             errors.Add(new Error("Street is required and must have up to 75 characters")
                .WithMetadata("Field", nameof(street)));
        }
    }
   
    private static void ValidateNumber(string number, List<Error> errors)
    {
        if (string.IsNullOrWhiteSpace(number) || number.Length > 10)
        {
             errors.Add(new Error("Number is required and must have up to 10 characters")
                .WithMetadata("Field", nameof(number)));
        }
    }
        
}