using FluentResults;
using Microworkers.Domain.Core.Exceptions;
using Microworkers.Domain.Core.Validations;

namespace Microworkers.Domain.Core.ValueObjects;

public record Address
{
    private Address() { }

    public string State { get; init; }
    public string ZipCode { get; init; }
    public string City { get; init; }
    public string NeighborHood { get; init; }
    public string Street { get; init; }
    public string Number { get; init; }
    public string? Additional { get; init; } // Único opcional

    public Address With(
    string? state = null,
    string? zipCode = null,
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

    public static Address Create(
        string state,
        string zipCode,
        string city,
        string neighborHood,
        string street,
        string number,
        string? additional = null)
    {
        var address = new Address
        {
            State = state,
            ZipCode = zipCode,
            City = city,
            NeighborHood = neighborHood,
            Street = street,
            Number = number,
            Additional = additional
        };
        var validationResult = new AddressValidator();
        var validation = validationResult.Validate(address);
        if (!validation.IsValid)
            throw new InvalidAddressDomainException(string.Join(";", validation.Errors));

        return address;
    }

    public static Result<Address> Update(
        Address original,
        string? pState = null,
        string? pZipCode = null,
        string? pCity = null,
        string? pNeighborHood = null,
        string? pStreet = null,
        string? pNumber = null,
        string? pAdditional = null)
    {
        return new Address
        {
            State = pState ?? original.State ,
            ZipCode = pZipCode != null ? pZipCode  : original.ZipCode,
            City = pCity ?? original.City,
            NeighborHood = pNeighborHood ?? original.NeighborHood,
            Street = pState ?? original.Street,
            Number = pNumber ?? original.Number,
            Additional = pAdditional ?? original.Additional
        };
    }
}