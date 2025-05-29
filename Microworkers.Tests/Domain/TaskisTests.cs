using Bogus;
using Microworkers.Domain.Core.Aggregates;
using Microworkers.Domain.Core.Enums;
using Microworkers.Domain.Core.Exceptions;
using Microworkers.Domain.Core.Factories;
using Microworkers.Domain.Shared;
using Microworkers.Tests.TestData;

namespace Microworkers.Tests.Domain;
public class TaskisTests
{

    [Fact]
    public void Given_UserHasThreeInProgressTask_When_AddingAnotherTask_The_ThrowsDomainException()
    {
        User customer = UserTestData.GenerateValidUser();
        User serviceProvider = UserTestData.GenerateValidUser();

        Result<Taski> taski1 = TaskiFactory.Create(customer.Id, customer, Guid.NewGuid(), "Task 1");
        Result userResult1 = customer.AddTaski(taski1.Value);
        taski1.Value.StartTask();

        Result<Taski> taski2 = TaskiFactory.Create(customer.Id, customer, Guid.NewGuid(), "Task 2" );
        Result userResult2 = customer.AddTaski(taski2.Value);
        taski2.Value.StartTask();

        Result<Taski> taski3 = TaskiFactory.Create(customer.Id, customer, Guid.NewGuid(), "Task 3" );
        Result userResult3 = customer.AddTaski(taski3.Value);
        taski3.Value.StartTask();

        // Act
        Result<Taski> taski4 = TaskiFactory.Create(customer.Id, customer, Guid.NewGuid(), "Task 4");
        Result userResult4 = null;
        string  expectedMessage = "Taski cannot be null";
        string? errorMessage = Assert.Throws<DomainException>(() => 
        {
            userResult4 = customer.AddTaski(taski4.Value);
        }).Message;

        // Assert
        Assert.True(taski1.IsSuccess);
        Assert.True(taski2.IsSuccess);
        Assert.True(taski3.IsSuccess);
        Assert.True(taski4.IsFailure);
        Assert.Null(userResult4);
        Assert.Equal(expectedMessage, errorMessage);
        Assert.Equal("User cannot have more than 3 tasks in progress", taski4.Error);
    }

    [Fact]
    public void Given_UserHasLessThanThreeInProgressTask_When_AddingAnotherTask_The_TaskIsAddedSuccessfully()
    {
        User customer = UserTestData.GenerateValidUser();
        User serviceProvider = UserTestData.GenerateValidUser();
        Result<Taski> taski1 = TaskiFactory.Create(customer.Id, customer, Guid.NewGuid(), "Task 1");
        Result userResult1 = customer.AddTaski(taski1.Value);
        taski1.Value.StartTask();
        Result<Taski> taski2 = TaskiFactory.Create(customer.Id, customer, Guid.NewGuid(), "Task 2");
        Result userResult2 = customer.AddTaski(taski2.Value);
        taski2.Value.StartTask();
        // Act
        Result<Taski> taski3 = TaskiFactory.Create(customer.Id, customer, Guid.NewGuid(), "Task 3");
        Result userResult3 = customer.AddTaski(taski3.Value);
        // Assert
        Assert.True(taski1.IsSuccess);
        Assert.True(taski2.IsSuccess);
        Assert.True(taski3.IsSuccess);
        Assert.NotNull(userResult3);
    }

    [Fact]
    public void Given_TaskiWithoutRequiredSkill_When_CreatingTaski_The_ThrowsDomainException()
    {
        //Arrange   
        User customer = UserTestData.GenerateValidUser();
        User serviceProvider = UserTestData.GenerateValidUser();
        string expectedMessage = "Task without required skill";
        // Act
        Result<Taski> taskiResult = TaskiFactory.Create(customer.Id, serviceProvider, Guid.Empty, "Task description");
        
        // Assert
        Assert.True(taskiResult.IsFailure);
        Assert.Equal(expectedMessage, taskiResult.Error);
    }

    [Fact]
    public void Given_TaskiWithoutServiceProvider_When_CreatingTaski_The_ThrowsDomainException()
    {
        //Arrange
        User customer = UserTestData.GenerateValidUser();
        User serviceProvider = null;
        string expectedMessage = "ServiceProvider cannot be null";
        // Act
        Result<Taski> taskiResult = TaskiFactory.Create(customer.Id, serviceProvider, Guid.NewGuid(), "Task description");
        
        // Assert
        Assert.True(taskiResult.IsFailure);
        Assert.Equal(expectedMessage, taskiResult.Error);
    }

    [Fact]
    public void Given_TaskiWithoutCustomer_When_CreatingTaski_The_ThrowsDomainException()
    {
        // Arrange
        User serviceProvider = UserTestData.GenerateValidUser();
        string expectedMessage = "Customer cannot be empty";
        // Act
        Result<Taski> taskiResult = TaskiFactory.Create(Guid.Empty, serviceProvider, Guid.NewGuid(), "Task description");
        
        // Assert
        Assert.True(taskiResult.IsFailure);
        Assert.Equal(expectedMessage, taskiResult.Error);
    }

    [Fact]
    public void Given_TaskiWithEmptyDescription_When_CreatingTaski_The_ThrowsDomainException()
    {
        // Arrange
        User customer = UserTestData.GenerateValidUser();
        User serviceProvider = UserTestData.GenerateValidUser();
        string expectedMessage = "Task description cannot be empty";
        
        // Act
        Result<Taski> taskiResult = TaskiFactory.Create(customer.Id, serviceProvider, Guid.NewGuid(), string.Empty);
        
        // Assert
        Assert.True(taskiResult.IsFailure);
        Assert.Equal(expectedMessage, taskiResult.Error);
    }

    [Fact]
    public void Given_TaskiWithMore500CharactersDescription_When_CreatingTaski_The_TaskiIsCreatedSuccessfully()
    {
        // Arrange
        User customer = UserTestData.GenerateValidUser();
        User serviceProvider = UserTestData.GenerateValidUser();
        Faker faker = new Faker();
        string message = faker.Lorem.Sentence(501);  
        string expectedMessage = "Task description cannot exceed 500 characters";   

        // Act
        Result<Taski> taskiResult = TaskiFactory.Create(customer.Id, serviceProvider, Guid.NewGuid(), message);
        
        // Assert
        Assert.True(taskiResult.IsFailure);
        Assert.Null(taskiResult.Value);
        Assert.Equal(expectedMessage, taskiResult.Error);
    }

    [Fact]
    public void Given_TaskiWithNullDescription_When_CreatingTaski_The_TaskiIsCreatedSuccessfully()
    {
        // Arrange
        User customer = UserTestData.GenerateValidUser();
        User serviceProvider = UserTestData.GenerateValidUser();
        string expectedMessage = "Task description cannot be empty or null";    

        // Act
        Result<Taski> taskiResult = TaskiFactory.Create(customer.Id, serviceProvider, Guid.NewGuid(), null);
        
        // Assert
        Assert.True(taskiResult.IsFailure);
        Assert.Null(taskiResult.Value);
        Assert.Equal(expectedMessage, taskiResult.Error);
    }

}
