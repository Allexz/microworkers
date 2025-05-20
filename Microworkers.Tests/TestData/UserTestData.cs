using Bogus;
using Bogus.Extensions.Brazil;
using Microworkers.Domain.Core.Aggregates;
using Microworkers.Domain.Core.ValueObjects;

namespace Microworkers.Tests.TestData;
public static class UserTestData
{
    public static User GenerateValidUser()
    {
        var faker = new Faker();
        return User.Create(
        Guid.NewGuid(),
        faker.Person.FullName,
        faker.Person.Cpf(),
        password: $"Test@{faker.Random.Number(100, 999)}",
        phone: Phone.Create($"({faker.Random.Number(10, 99):00})" +       // (dd)
               $"{faker.Random.Number(10000, 99999):00000}-" + // ddddd-
               $"{faker.Random.Number(1000, 9999):0000}"),     // dddd
        username: faker.Person.Email,
        address: Address.Create(
            state: faker.Address.State(),
        zipCode: faker.Address.ZipCode("#####-###"),
        city: faker.Address.City(),
        neighborHood: faker.Address.County(),
        street: faker.Address.StreetName(),
        number: faker.Address.BuildingNumber())
        );
    }

    public static User GenerateUserWithInvalidUserName()
    {
        var faker = new Faker();
        return User.Create(Guid.NewGuid(),
        faker.Person.FullName,
        faker.Person.Cpf(),
        password: $"Test@{faker.Random.Number(100, 999)}",
        phone: Phone.Create($"({faker.Random.Number(10, 99):00})" +       // (dd)
               $"{faker.Random.Number(10000, 99999):00000}-" + // ddddd-
               $"{faker.Random.Number(1000, 9999):0000}"),     // dddd
        username: faker.Random.Word(),
        address: AddressTestData.GenerateValidAddress()) ;
    }

    public static User GenerateUserWithInvalidName()
    {
        var faker = new Faker();
        return User.Create(Guid.NewGuid(),
        " ",
        faker.Person.Cpf(),
        password: $"Test@{faker.Random.Number(100, 999)}",
        phone: Phone.Create($"({faker.Random.Number(10, 99):00})" +       // (dd)
               $"{faker.Random.Number(10000, 99999):00000}-" + // ddddd-
               $"{faker.Random.Number(1000, 9999):0000}"),     // dddd
        username: faker.Person.Email,
        address: AddressTestData.GenerateValidAddress());
         
    }

    public static User GenerateUserWithInvalidLongName()
    {
        var faker = new Faker();
        return User.Create(Guid.NewGuid(),
        string.Join(" ", faker.Lorem.Words(100)),
        faker.Person.Cpf(),
        password: $"Test@{faker.Random.Number(100, 999)}",
        phone: Phone.Create($"({faker.Random.Number(10, 99):00})" +       // (dd)
               $"{faker.Random.Number(10000, 99999):00000}-" + // ddddd-
               $"{faker.Random.Number(1000, 9999):0000}"),     // dddd
        username: faker.Person.Email,
        address: AddressTestData.GenerateValidAddress());
    }

    public static User GenerateUserWithInvalidCpf()
    {
        var faker = new Faker();
        return User.Create(
        Guid.NewGuid(),
        faker.Person.FullName,
        "123456789",
        password: $"Test@{faker.Random.Number(100, 999)}",
        phone: Phone.Create($"({faker.Random.Number(10, 99):00})" +       // (dd)
               $"{faker.Random.Number(10000, 99999):00000}-" + // ddddd-
               $"{faker.Random.Number(1000, 9999):0000}"),     // dddd
        username: faker.Person.Email,
        address: AddressTestData.GenerateValidAddress());
    }

    public static User GenerateUserWithInvalidPhone()
    {
        var faker = new Faker();
        return User.Create(
        Guid.NewGuid(),
        faker.Person.FullName,
        faker.Person.Cpf(),
        password: $"Test@{faker.Random.Number(100, 999)}",
        phone: Phone.Create($"({faker.Random.Number(10, 99):00})"),
        username: faker.Person.Email,
        address: AddressTestData.GenerateValidAddress());
    }


}
