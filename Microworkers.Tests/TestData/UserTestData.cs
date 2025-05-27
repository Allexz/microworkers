using Bogus;
using Bogus.Extensions.Brazil;
using Microworkers.Domain.Core.Aggregates;
using Microworkers.Domain.Core.Factories;
using Microworkers.Domain.Core.ValueObjects;
using Microworkers.Domain.Shared;

namespace Microworkers.Tests.TestData;
public static class UserTestData
{
    private static Faker faker = new Faker("pt_BR");    
    private static Phone phone = Phone.Create($"({faker.Random.Number(10, 99):00})" +       // (dd)
               $"{faker.Random.Number(10000, 99999):00000}-" + // ddddd-
               $"{faker.Random.Number(1000, 9999):0000}").Value;
    public static User GenerateValidUser()
    {
        Faker faker = new Faker("pt_BR");
        string name = faker.Person.FullName;
        string username = faker.Person.Email;
        Phone phone = Phone.Create($"({faker.Random.Number(10, 99):00})" +       // (dd)
               $"{faker.Random.Number(10000, 99999):00000}-" + // ddddd-
               $"{faker.Random.Number(1000, 9999):0000}").Value;


        return UserFactory.Create(
        Guid.NewGuid(),
        name.Length > 75
            ? name.Substring(0,75)
        : name,
        DocumentFactory.Create(faker.Person.Cpf(),Microworkers.Domain.Core.Enums.DocumentType.CPF).Value,
        password: $"XPTOzy1234****",
        phone: phone,     // dddd
        username: username.Length > 75 
            ? username.Substring(0,75)
            : username,
        address: AddressTestData.GenerateValidAddress()).Value;
    }

    public static Result<User> GenerateUserWithInvalidUserName()
    {
        var faker = new Faker();
        return  UserFactory.Create(Guid.NewGuid(),
        faker.Person.FullName,
        DocumentFactory.Create( faker.Person.Cpf(), Microworkers.Domain.Core.Enums.DocumentType.CPF).Value,
        password: $"Test@{faker.Random.Number(100, 999)}",
        phone: phone,      
        username: faker.Random.Word(),
        address: AddressTestData.GenerateValidAddress()) ;
    }

    public static Result<User> GenerateUserWithInvalidName()
    {
        var faker = new Faker();
        return UserFactory.Create(Guid.NewGuid(),
        " ",
        DocumentFactory.Create(faker.Person.Cpf(), Microworkers.Domain.Core.Enums.DocumentType.CPF).Value,
        password: $"Test@{faker.Random.Number(100, 999)}",
        phone: phone,     // dddd
        username: faker.Person.Email,
        address: AddressTestData.GenerateValidAddress());
         
    }

    public static Result<User> GenerateUserWithInvalidLongName()
    {
        var faker = new Faker();
        return UserFactory.Create(Guid.NewGuid(),
        string.Join(" ", faker.Lorem.Words(100)),
        DocumentFactory.Create(faker.Person.Cpf(), Microworkers.Domain.Core.Enums.DocumentType.CPF).Value,
        password: $"Test@{faker.Random.Number(100, 999)}",
        phone: phone,     // dddd
        username: faker.Person.Email,
        address: AddressTestData.GenerateValidAddress());
    }

    public static Result<User> GenerateUserWithInvalidCpf()
    {
        Result<Document> documentResult = DocumentFactory.Create("123456", Microworkers.Domain.Core.Enums.DocumentType.CPF);

        if (documentResult.IsFailure)
            return Result.Fail<User>(documentResult.Error);

        var faker = new Faker();
        return UserFactory.Create(
        Guid.NewGuid(),
        faker.Person.FullName,
         documentResult.Value ,
        password: $"Test@{faker.Random.Number(100, 999)}",
        phone: phone,     // dddd
        username: faker.Person.Email,
        address: AddressTestData.GenerateValidAddress());
    }

    public static Result<User> GenerateUserWithInvalidPhone()
    {
        var faker = new Faker();
        Result<Phone> resultPhone = Phone.Create($"({faker.Random.Number(10, 99):00})");

        if (resultPhone.IsFailure)
            return Result.Fail<User>(resultPhone.Error);

        return UserFactory.Create(
        Guid.NewGuid(),
        faker.Person.FullName,
        DocumentFactory.Create(faker.Person.Cpf(), Microworkers.Domain.Core.Enums.DocumentType.CPF).Value,
        password: $"Test@{faker.Random.Number(100, 999)}",
        phone:  resultPhone.Value,
        username: faker.Person.Email,
        address: AddressTestData.GenerateValidAddress());
    }


}
