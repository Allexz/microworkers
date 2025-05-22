using FluentResults;
using Microworkers.Domain.Core.Exceptions;
using System.Text.RegularExpressions;

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
        ValidateAddress(address);
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
        Address address = new Address
        {
            State = pState ?? original.State ,
            ZipCode = pZipCode != null ? pZipCode  : original.ZipCode,
            City = pCity ?? original.City,
            NeighborHood = pNeighborHood ?? original.NeighborHood,
            Street = pState ?? original.Street,
            Number = pNumber ?? original.Number,
            Additional = pAdditional ?? original.Additional
        };
        ValidateAddress(address);
        return address;
    }

    private static void ValidateAddress(Address address)
    {
        if (address is null)
            throw new InvalidAddressDomainException("Address cannot be null", nameof(address));

        if (string.IsNullOrWhiteSpace(address.State))
            throw new InvalidAddressDomainException("State cannot be null or empty", nameof(address.State));

        if (address.State.Length != 2)
            throw new InvalidAddressDomainException("State must be 2 characters long", nameof(address.State));

        if (string.IsNullOrWhiteSpace(address.ZipCode))
            throw new InvalidAddressDomainException("Zipcode cannot be null or empty", nameof(address.ZipCode));

        if (!Regex.IsMatch(address.ZipCode, @"^\d{5}-\d{3}$"))
            throw new InvalidAddressDomainException("Zipcode must be in the format XXXXX-XXX", nameof(address.ZipCode));

        if (string.IsNullOrWhiteSpace(address.City))
            throw new InvalidAddressDomainException("City cannot be null or empty", nameof(address.City));

        if (address.City.Length > 75)
            throw new InvalidAddressDomainException("City must be at most 75 characters long", nameof(address.City));

        if (string.IsNullOrWhiteSpace(address.NeighborHood))
            throw new InvalidAddressDomainException("Neighborhood cannot be null or empty", nameof(address.NeighborHood));

        if (address.NeighborHood.Length > 30)
            throw new InvalidAddressDomainException("Neighborhood must be at most 30 characters long", nameof(address.NeighborHood));

        if (string.IsNullOrWhiteSpace(address.Street))
            throw new InvalidAddressDomainException("Street cannot be null or empty", nameof(address.Street));

        if (address.Street.Length > 75)
            throw new InvalidAddressDomainException("Street must be at most 75 characters long", nameof(address.Street));

        if (string.IsNullOrWhiteSpace(address.Number))
            throw new InvalidAddressDomainException("Number cannot be null or empty", nameof(address.Number));

        if (address.Number.Length > 10)
            throw new InvalidAddressDomainException("Number must be at most 10 characters long", nameof(address.Number));

        if (address.Additional != null && address.Additional.Length > 30)
            throw new InvalidAddressDomainException("Additional must be at most 30 characters long", nameof(address.Additional));
    }
}