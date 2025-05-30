using Bogus;
using Microworkers.Domain.Core.Factories;
using Microworkers.Domain.Core.ValueObjects;
using Microworkers.Domain.Shared;
using Microworkers.Tests.TestData;

namespace Microworkers.Tests.Domain;
public class ProposalsTest
{
    [Fact]
    public void Given_InvalidProposalWithEmptyDescription_When_Validate_Should_ReturnResultError()
    {
        // Arrange
        var faker = new Faker();

        // Act
        var resultProposal = ProposalFactory.Create(
            task: TaskiTestData.GenerateValidTaski(),
            serviceProvider: Guid.NewGuid(),
            jobDescription: ProposalDescription.Create(string.Empty).Value,
            price: faker.Finance.Amount(100, 1000),
            projectedDate: DateTime.UtcNow.AddDays(faker.Random.Int(1, 30))
        );

        // Assert
        Assert.True(resultProposal.IsFailure);
        Assert.Equal("JobDescription cannot be null", resultProposal.Error);
    }

    [Fact]
    public void Given_InvalidProposalWithLongDescription_When_Validate_Should_ReturnResultError()
    {
        // Arrange
        var faker = new Faker();
        // Act
        Result<ProposalDescription> resultJobDescription = ProposalDescription.Create(faker.Random.String2(501));

        // Assert
        Assert.True(resultJobDescription.IsFailure);
        Assert.Equal("Proposal description cannot exceed 500 characters", resultJobDescription.Error);
    }

    [Fact]
    public void Given_InvalidProposalWithNullDescription_When_Validate_Should_ReturnResultError()
    {
        // Arrange
        var faker = new Faker();

        // Act
        var resultProposal = ProposalFactory.Create(
            task: TaskiTestData.GenerateValidTaski(),
            serviceProvider: Guid.NewGuid(),
            jobDescription: ProposalDescription.Create(faker.Random.String2(501)).Value,
            price: faker.Finance.Amount(100, 1000),
            projectedDate: DateTime.UtcNow.AddDays(faker.Random.Int(1, 30))
        );

        // Assert
        Assert.True(resultProposal.IsFailure);
        Assert.Equal("JobDescription cannot be null", resultProposal.Error);
    }

    [Fact]
    public void Given_InvalidProposalWithNullTask_When_Validate_Should_ReturnResultError()
    {
        var faker = new Faker();
        ProposalDescription proposalDesc = ProposalDescription.Create(faker.Random.String2(400)).Value;
        var resultProposal = ProposalFactory.Create(
            task: null,
            serviceProvider: Guid.NewGuid(),
            jobDescription: proposalDesc ,
            price: faker.Finance.Amount(100, 1000),
            projectedDate: DateTime.UtcNow.AddDays(faker.Random.Int(1, 30))
        );
        Assert.True(resultProposal.IsFailure);
        Assert.Equal("Task cannot be null", resultProposal.Error);
    }

    [Fact]
    public void Given_InvalidProposalWithEmptyServiceProvider_When_Validate_Should_ReturnResultError()
    {
        // Arrange
        var faker = new Faker();
        ProposalDescription proposalDesc = ProposalDescription.Create(faker.Random.String2(400)).Value;
        
        // Act
        var resultProposal = ProposalFactory.Create(
            task: TaskiTestData.GenerateValidTaski(),
            serviceProvider: Guid.Empty,
            jobDescription: proposalDesc,
            price: faker.Finance.Amount(100, 1000),
            projectedDate: DateTime.UtcNow.AddDays(faker.Random.Int(1, 30))
        );
        // Assert
        Assert.True(resultProposal.IsFailure);
        Assert.Equal("ServiceProvider cannot be empty", resultProposal.Error);
    }

    [Fact]
    public void Given_InvalidProposalWithZeroPrice_When_Validate_Should_ReturnResultError()
    {
        // Arrange
        var faker = new Faker();
        ProposalDescription proposalDesc = ProposalDescription.Create(faker.Random.String2(400)).Value;
        
        // Act
        var resultProposal = ProposalFactory.Create(
            task: TaskiTestData.GenerateValidTaski(),
            serviceProvider: Guid.NewGuid(),
            jobDescription: proposalDesc,
            price: 0,
            projectedDate: DateTime.UtcNow.AddDays(faker.Random.Int(1, 30))
        );
        
        // Assert
        Assert.True(resultProposal.IsFailure);
        Assert.Equal("Price must be greater than zero", resultProposal.Error);
    }

}
