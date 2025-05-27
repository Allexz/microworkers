using Bogus;
using Microworkers.Domain.Core.Aggregates;
using Microworkers.Domain.Core.Exceptions;
using Microworkers.Domain.Core.Factories;
using Microworkers.Domain.Core.ValueObjects;
using Microworkers.Domain.Shared;
using Microworkers.Tests.TestData;

namespace Microworkers.Tests.Domain;
public class UserTest
{
    [Fact]
    public void Given_New_User_When_Username_Invalid_Format_Then_Throw_Exception()
    {
        Faker faker = new Faker();
        var expectedMessage = "Username must be a valid email address";
        Result<User> resultUser = UserTestData.GenerateUserWithInvalidUserName();
        Assert.Contains(expectedMessage, resultUser.Error);
    }

    [Fact]
    public void Given_New_User_When_Name_Invalid_Format_Then_Throw_Exception()
    {
        Faker faker = new Faker();
        var expectedMessage = "Name cannot be empty";
        Result<User> resultUser = UserTestData.GenerateUserWithInvalidName();
        Assert.Contains(expectedMessage, resultUser.Error);
    }

    [Fact]
    public void Given_New_User_When_Name_Is_Too_Long_Then_Throw_Exception()
    {
        Faker faker = new Faker();
        var expectedMessage = "Name must be at least 3 character and cannot be longer than 75.";
        Result<User> resultUser = UserTestData.GenerateUserWithInvalidLongName();
        Assert.Contains(expectedMessage, resultUser.Error);
    }

    [Fact]
    public void Give_New_User_When_CPF_Invalid_Then_Throw_Exception()
    {
        Faker faker = new Faker();
        var expectedMessage = "Document is not a valid CPF";
        Result<User> resultUser = UserTestData.GenerateUserWithInvalidCpf();
        Assert.Contains(expectedMessage, resultUser.Error);
    }

    [Fact]
    public void Give_New_User_When_Phone_Invalid_Then_Throw_Exception()
    {
        Faker faker = new Faker();
        var expectedMessage = "Phone must be in the format (XX)XXXXX-XXXX";
        Result<User> resultUser  =  UserTestData.GenerateUserWithInvalidPhone() ;
        Assert.Contains(expectedMessage, resultUser.Error);
    }

    [Theory]
    [InlineData("No", null, null, null, null, null, null)]
    [InlineData(null, "98765-432", null, null, null, null, null)]
    [InlineData(null, null, "Nova Cidade", null, null, null, null)]
    [InlineData(null, null, null, "Novo Bairro", null, null, null)]
    [InlineData(null, null, null, null, "Nova Rua", null, null)]
    [InlineData(null, null, null, null, null, "999", null)]
    [InlineData(null, null, null, null, null, null, "Novo Complemento")]
    public void With_ShouldUpdateOnlySpecifiedFields(string newState = null,
                                                     string newZipCode = null,
                                                     string newCity = null,
                                                     string newNeighborHood = null,
                                                     string newStreet = null,
                                                     string newNumber = null,
                                                     string newAdditional = null)
    {
        // Arrange
        var original = AddressTestData.GenerateValidAddress();

        // Act
        Address updated = AddressFactory.With(
            original,
            state: newState,
            zipCode: newZipCode,
            city: newCity,
            neighborHood: newNeighborHood,
            street: newStreet,
            number: newNumber,
            additional: newAdditional
        ).Value;

        // Assert
        if (newState != null)
            Assert.NotEqual(original.State, updated.State);

        if (newZipCode != null)
            Assert.NotEqual(original.ZipCode, updated.ZipCode);

        if (newCity != null)
            Assert.NotEqual(original.City, updated.City);

        if (newNeighborHood != null)
            Assert.NotEqual(original.NeighborHood, updated.NeighborHood);

        if (newStreet != null)
            Assert.NotEqual(original.Street, updated.Street);

        if (newNumber != null)
            Assert.NotEqual(original.Number, updated.Number);

        if (newAdditional != null)
            Assert.NotEqual(original.Additional, updated.Additional);
    }

    [Theory]
    [InlineData(null, "12345-678", "São Paulo", "Centro", "Rua A", "123")] // State nulo
    [InlineData("SP", null, "São Paulo", "Centro", "Rua A", "123")] // ZipCode nulo
    [InlineData("SP", "12345-678", null, "Centro", "Rua A", "123")] // City nulo
    [InlineData("SP", "12345-678", "São Paulo", null, "Rua A", "123")] // Neighborhood nulo
    [InlineData("SP", "12345-678", "São Paulo", "Centro", null, "123")] // Street nulo
    [InlineData("SP", "12345-678", "São Paulo", "Centro", "Rua A", null)] // Number nulo
    public void Given_Invalid_Address_When_Create_Results_Should_Have_Notifications(
    string state, string zipCode, string city,
    string neighborHood, string street, string number)
    {
        // Arrange
        Result<Address> result = AddressFactory.Create(state, zipCode, city, neighborHood, street, number);
        Assert.False(result.IsSuccess);
    }

}
