using Microworkers.Domain.Core.Aggregates;
using Microworkers.Domain.Core.Events;
using Microworkers.Domain.Core.ValueObjects;
using Microworkers.Domain.Shared;

namespace Microworkers.Domain.Core.Factories;
public static class UserFactory
{
    public static Result<User> Create(Guid id, string name, Document document, string password, Phone phone, string username, Address address)
    {
        var errors = new List<string>();

        // Name validation
        if (string.IsNullOrWhiteSpace(name))
            errors.Add("Name cannot be empty.");
        else if (name.Length < 3 || name.Length > 75)
            errors.Add("Name must be at least 3 character and cannot be longer than 75.");

        // Document validation (CPF format: XXX.XXX.XXX-XX)
        Result<Document> documentResult = DocumentFactory.Create(document.Number, Enums.DocumentType.CPF);
        if (documentResult.IsFailure)
            errors.Add(documentResult.Error);
        
        // Password validation
        if (string.IsNullOrWhiteSpace(password))
            errors.Add("Password cannot be empty.");
        else if (password.Length < 10 || password.Length > 100)
            errors.Add("Password must be at least 10 character and cannot be longer than 100");

        // Phone validation (delegates to Phone.Create)
        Result<Phone> phoneResult = Phone.Create(phone?.Number ?? "");
        if (phoneResult.IsFailure)
            errors.Add(phoneResult.Error);

        // Username validation (email)
        if (string.IsNullOrWhiteSpace(username))
            errors.Add("Username cannot be null nor empty");
        else if (!System.Text.RegularExpressions.Regex.IsMatch(username, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            errors.Add("Username must be a valid email address");

        // Address validation (delegates to Address.Create)
        Result<Address> addressResult = AddressFactory.Create(
            address?.State ?? "",
            address?.ZipCode ?? "",
            address?.City ?? "",
            address?.NeighborHood ?? "",
            address?.Street ?? "",
            address?.Number ?? "",
            address?.Additional
        );
        if(addressResult.IsFailure)
            errors.Add(addressResult.Error);

        if (errors.Any())
            return Result.Fail<User>(string.Join("; ", errors));

        var user = new User(id, name, document, password, phoneResult.Value, username, addressResult.Value);
        user.AddDomainEvent(new UserCreatedEvent(user.Id, user.Name, document, user.Phone, user.Username));
        return Result.Ok(user);
    }
}
