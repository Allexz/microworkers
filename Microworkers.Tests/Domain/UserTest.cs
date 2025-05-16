using FluentAssertions;
using FluentResults;
using Microworkers.Domain.Core.Aggregates;
using Microworkers.Domain.Core.Exceptions;
using Microworkers.Domain.Core.ValueObjects;

namespace Microworkers.Tests.Domain;
public class UserTest
{
    [Fact]
    public void Should_Not_Create_User_If_Username_Isnt_Email()
    {
        Result<User> result =  
            User.Create(Guid.NewGuid(),
           "John Doe"
           , "123456789"
           , "password1234"
           , Phone.Create(123456789).Value
           , "johndoe");

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(error =>
            error.Message == "Invalid email format" &&
            error.Metadata["Field"].ToString() =="username");
    }


    [Fact]
    public void Should_Not_Create_User_Phone()
    {
        Result<Phone> result = Phone.Create(12345678);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(error =>
            error.Message == "Phone number must have at 9 digits." &&
            error.Metadata["Field"].ToString() == "number");
    }

    [Fact]
    public void Should_Not_Create_User_Address_StateError()
    {
        Result<Address> result = Address.Create(
            "SPA",
            12345678,
            "São Paulo",
            "Centro",
            "Rua das Flores",
            "123");

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(error =>
            error.Message == "State is required and must have exactly 2 characters" &&
            error.Metadata["Field"].ToString() == "state");
    }

    [Fact]
    public void Should_Not_Create_User_Address_ZipCodeError()
    {
        Result<Address> result = Address.Create(
            "SP",
            123456789,
            "São Paulo",
            "Centro",
            "Rua das Flores",
            "123");

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(error =>
            error.Message == "ZipCode must have exactly 8 digits" &&
            error.Metadata["Field"].ToString() == "zipCode");
    }

    [Fact]
    public void Should_Not_Create_User_Address_NumberError()
    {
        Result<Address> result = Address.Create(
            "SP",
            12345678,
            "São Paulo",
            "Centro",
            "Rua das Flores",
            "123555555555555555555");

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(error =>
            error.Message == "Number is required and must have up to 10 characters" &&
            error.Metadata["Field"].ToString() == "number");
    }

    [Fact]
    public void Should_Not_Update_User_Address_NumberError()
    {
        Address address = Address.Create(
            "SP",
            12345678,
            "São Paulo",
            "Centro",
            "Rua das Flores",
            "123").Value;

        Result<Address> result = Address.Update(
            address,
            pNumber: "123555555555555555555");
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(error =>
            error.Message == "Number is required and must have up to 10 characters" &&
            error.Metadata["Field"].ToString() == "number");
    }


}
