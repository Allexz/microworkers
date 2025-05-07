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
}
