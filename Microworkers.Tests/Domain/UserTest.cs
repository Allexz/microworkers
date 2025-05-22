using Bogus;
using Microworkers.Domain.Core.Aggregates;
using Microworkers.Domain.Core.Exceptions;
using Microworkers.Domain.Core.ValueObjects;
using Microworkers.Tests.TestData;

namespace Microworkers.Tests.Domain;
public class UserTest
{
    [Fact]
    public void Given_New_User_When_Username_Invalid_Format_Then_Throw_Exception()
    {
        Faker faker = new Faker();
        var expectedMessage = "Username must be a valid email address";
        var exception = Assert.Throws<InvalidUserDomainException>(  
            () => UserTestData.GenerateUserWithInvalidUserName());
        Assert.Contains(expectedMessage, exception.Message);
    }

    [Fact]
    public void Given_New_User_When_Name_Invalid_Format_Then_Throw_Exception()
    {
        Faker faker = new Faker();
        var expectedMessage = "Name cannot be empty";
        var exception = Assert.Throws<InvalidUserDomainException>(
            () => UserTestData.GenerateUserWithInvalidName());
        Assert.Contains(expectedMessage, exception.Message);
    }

    [Fact]
    public void Given_New_User_When_Name_Is_Too_Long_Then_Throw_Exception()
    {
        Faker faker = new Faker();
        var expectedMessage = "Name must be at least 3 character and cannot be longer than 75.";
        var exception = Assert.Throws<InvalidUserDomainException>(
            () => UserTestData.GenerateUserWithInvalidLongName());
        Assert.Contains(expectedMessage, exception.Message);
    }

    [Fact]
    public void Give_New_User_When_CPF_Invalid_Then_Throw_Exception()
    {
        Faker faker = new Faker();
        var expectedMessage = "Document must be in the format XXX.XXX.XXX-XX.";
        var exception = Assert.Throws<InvalidUserDomainException>(
            () => UserTestData.GenerateUserWithInvalidCpf());
        Assert.Contains(expectedMessage, exception.Message);
    }

    [Fact]
    public void Give_New_User_When_Phone_Invalid_Then_Throw_Exception()
    {
        Faker faker = new Faker();
        var expectedMessage = "Phone must be in the format (XX)XXXXX-XXXX";
        var exception = Assert.Throws<InvalidPhoneException>(
            () => UserTestData.GenerateUserWithInvalidPhone());
        Assert.Contains(expectedMessage, exception.Message);
    }

    [Theory]
    [InlineData("Novo Estado", null, null, null, null, null, null)]
    [InlineData(null, "98765-432", null, null, null, null, null)]
    [InlineData(null, null, "Nova Cidade", null, null, null, null)]
    [InlineData(null, null, null, "Novo Bairro", null, null, null)]
    [InlineData(null, null, null, null, "Nova Rua", null, null)]
    [InlineData(null, null, null, null, null, "999", null)]
    [InlineData(null, null, null, null, null, null, "Novo Complemento")]
    public void With_ShouldUpdateOnlySpecifiedFields(
    string? newState, string? newZipCode, string? newCity,
    string? newNeighborHood, string? newStreet, string? newNumber, string? newAdditional)
    {
        // Arrange
        var original = AddressTestData.GenerateValidAddress();

        // Act
        var updated = original.With(
            state: newState,
            zipCode: newZipCode,
            city: newCity,
            neighborHood: newNeighborHood,
            street: newStreet,
            number: newNumber,
            additional: newAdditional
        );

        // Assert
        Assert.Equal(newState ?? original.State, updated.State);
        Assert.Equal(newZipCode ?? original.ZipCode, updated.ZipCode);
        Assert.Equal(newCity ?? original.City, updated.City);
        Assert.Equal(newNeighborHood ?? original.NeighborHood, updated.NeighborHood);
        Assert.Equal(newStreet ?? original.Street, updated.Street);
        Assert.Equal(newNumber ?? original.Number, updated.Number);
        Assert.Equal(newAdditional ?? original.Additional, updated.Additional);
    }

    [Theory]
    [InlineData(null, "12345-678", "São Paulo", "Centro", "Rua A", "123")] // State nulo
    [InlineData("SP", null, "São Paulo", "Centro", "Rua A", "123")] // ZipCode nulo
    [InlineData("SP", "12345-678", null, "Centro", "Rua A", "123")] // City nulo
    [InlineData("SP", "12345-678", "São Paulo", null, "Rua A", "123")] // Neighborhood nulo
    [InlineData("SP", "12345-678", "São Paulo", "Centro", null, "123")] // Street nulo
    [InlineData("SP", "12345-678", "São Paulo", "Centro", "Rua A", null)] // Number nulo
    public void Create_WithInvalidData_ShouldThrowException(
    string state, string zipCode, string city,
    string neighborHood, string street, string number)
    {
        // Act & Assert
        Assert.Throws<InvalidAddressDomainException>(() =>
            Address.Create(state, zipCode, city, neighborHood, street, number));
    }

    [Fact]
    public void Given_Existing_User_When_Updating_With_Invalid_Data_Then_Throw_Exception()
    {
        // Arrange
        var faker = new Faker();
        var user = UserTestData.GenerateValidUser();
        var expectedMessage = "Phone must be in the format (XX)XXXXX-XXXX";
        // Act
        var exception = Assert.Throws<InvalidPhoneException>(
            () =>
            {
                User.With(user, phone: Phone.Create("222222"));
            });
        // Assert
        Assert.Contains(expectedMessage, exception.Message);
    }











}
