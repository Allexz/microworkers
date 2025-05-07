using Microworkers.Domain.Core.Aggregates;
using Microworkers.Domain.Core.Enums;
using Microworkers.Domain.Core.Exceptions;
using Microworkers.Domain.Core.ValueObjects;

namespace Microworkers.Tests.Domain;
public class ProposalsTest
{
    [Theory]
    [InlineData(TaskiStatus.InProgress)]
    [InlineData(TaskiStatus.Completed)]
    [InlineData(TaskiStatus.Cancelled)]
    public void Should_ThrowException_When_CreatingProposal_For_InvalidTaskiStatus(TaskiStatus status)
    {
        // Arrange
        List<Taski> taskis = new List<Taski>();
        var taski = Taski.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Task description", taskis);
        var jobDescription = ProposalDescription.Create("Proposal description");
        var serviceProvider = Guid.NewGuid();
        var price = 100m;
        var projectedDate = DateTime.UtcNow.AddDays(7);

        // Simulate the Taski status
        switch (status)
        {
            case TaskiStatus.InProgress:
                taski.StartTask();
                break;
            case TaskiStatus.Completed:
                taski.StartTask();
                taski.CompleteTask(TaskiResult.Success, "Task completed successfully");
                break;
            case TaskiStatus.Cancelled:
                taski.CancelTask("Task cancelled");
                break;
        }

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() =>
            Proposal.Create(taski, serviceProvider, jobDescription, price, projectedDate)
        );

        Assert.Equal("Cannot create a proposal for a task that is not in the Opened status", exception.Message);
    }

    [Fact]
    public void Should_CreateProposal()
    {
        // Arrange
        List<Taski> taskis = new List<Taski>();
        var taski = Taski.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Task description", taskis);
        var jobDescription = ProposalDescription.Create("Proposal description");
        var serviceProvider = Guid.NewGuid();
        var price = 100m;
        var projectedDate = DateTime.UtcNow.AddDays(7);

        
        // Act & Assert
        Proposal proposta = Proposal.Create(taski, serviceProvider, jobDescription, price, projectedDate);
        Assert.True(proposta.Status == ProposalStatus.Pending, "Proposal status should be Pending after creation");
        Assert.True(taski.Status == TaskiStatus.Opened, "Taski status should be Opened before creating a proposal");
         
    }

    [Theory]
    [InlineData("b3b375b5-32a5-479b-8dce-b9485041928e")]
    public void Should_Cancel_An_Proposal(string pServiceProvider)
    {
        Guid serviceProvider = Guid.Parse(pServiceProvider);
        // Arrange
        var taski = Taski.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Task description", new List<Taski>());
        var jobDescription = ProposalDescription.Create("Proposal description");
        var price = 100m;
        var projectedDate = DateTime.UtcNow.AddDays(7);

        Proposal proposta = Proposal.Create(taski, serviceProvider, jobDescription, price, projectedDate);

        // Act
        proposta.Cancel(serviceProvider);
        // Assert
        Assert.Equal(ProposalStatus.Cancelled, proposta.Status);
    }
}
