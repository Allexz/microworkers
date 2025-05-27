using Microworkers.Domain.Core.ValueObjects;
using Microworkers.Domain.Shared;
using System.Text.RegularExpressions;

namespace Microworkers.Domain.Core.Factories;
public static class AddressFactory
{
    public static Result<Address> Create(
        string state,
        string zipCode,
        string city,
        string neighborHood,
        string street,
        string number,
        string? additional = null)
    {
        var errors = new List<string>();

        // State: not null/empty, length 2
        if (string.IsNullOrWhiteSpace(state))
            errors.Add("State cannot be null nor empty");
        else if (state.Length != 2)
            errors.Add("State must be 2 characters long");

        // ZipCode: not null/empty, format XXXXX-XXX
        if (string.IsNullOrWhiteSpace(zipCode))
            errors.Add("ZipCode cannot be null nor empty");
        else if (!Regex.IsMatch(zipCode, @"^\d{5}-\d{3}$"))
            errors.Add("ZipCode must be in the format XXXXX-XXX");

        // City: not null/empty, max 75
        if (string.IsNullOrWhiteSpace(city))
            errors.Add("City cannot be null nor empty");
        else if (city.Length > 75)
            errors.Add("City must be at most 75 characters long");

        // NeighborHood: not null/empty, max 30
        if (string.IsNullOrWhiteSpace(neighborHood))
            errors.Add("Neighborhood cannot be null nor empty");
        else if (neighborHood.Length > 30)
            errors.Add("Neighborhood must be at most 30 characters long");

        // Street: not null/empty, max 75
        if (string.IsNullOrWhiteSpace(street))
            errors.Add("Street cannot be null nor empty");
        else if (street.Length > 75)
            errors.Add("Street must be at most 75 characters long");

        // Number: not null/empty, max 10
        if (string.IsNullOrWhiteSpace(number))
            errors.Add("Number cannot be null nor empty");
        else if (number.Length > 10)
            errors.Add("Number must be at most 10 characters long");

        // Additional: max 30 (optional)
        if (additional != null && additional.Length > 30)
            errors.Add("Additional must be at most 30 characters long");

        if (errors.Any())
            return Result.Fail<Address>(string.Join("; ", errors));

        var address = new Address(state, zipCode, city, neighborHood, street, number, additional);
        return Result.Ok(address);
    }


    public static Result<Address> With(
    Address currentAddress,
    string state = null,
    string zipCode = null,
    string city = null,
    string neighborHood = null,
    string street = null,
    string number = null,
    string? additional = null)
    {
        if (currentAddress == null)
            return Result.Fail<Address>("Current address cannot be null.");

        var errors = new List<string>();

        if (!string.IsNullOrWhiteSpace(state) && state.Length != 2)
            errors.Add("State must be 2 characters long");

        if (!string.IsNullOrWhiteSpace(zipCode) && !Regex.IsMatch(zipCode, @"^\d{5}-\d{3}$"))
            errors.Add("ZipCode must be in the format XXXXX-XXX");

        if (!string.IsNullOrWhiteSpace(city) && city.Length > 75)
            errors.Add("City must be at most 75 characters long");

        if (!string.IsNullOrWhiteSpace(neighborHood) && neighborHood.Length > 30)
            errors.Add("Neighborhood must be at most 30 characters long");

        if (!string.IsNullOrWhiteSpace(street) && street.Length > 75)
            errors.Add("Street must be at most 75 characters long");

        if (!string.IsNullOrWhiteSpace(number) && number.Length > 10)
            errors.Add("Number must be at most 10 characters long");

        if (additional != null && additional.Length > 30)
            errors.Add("Additional must be at most 30 characters long");

        if (errors.Any())
            return Result.Fail<Address>(string.Join("; ", errors));

        var newAddress = new Address(
            state ?? currentAddress.State,
            zipCode ?? currentAddress.ZipCode,
            city ?? currentAddress.City,
            neighborHood ?? currentAddress.NeighborHood,
            street ?? currentAddress.Street,
            number ?? currentAddress.Number,
            additional ?? currentAddress.Additional
        );

        return Result.Ok(newAddress);
    }

}
