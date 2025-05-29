using Microworkers.Domain.Core.Aggregates;
using Microworkers.Domain.Core.Enums;
using Microworkers.Domain.Core.Exceptions;
using Microworkers.Domain.Core.ValueObjects;

namespace Microworkers.Tests.Domain;
public class ProposalsTest
{
    [Fact]
    public void CreateProposal_ShouldThrowException_WhenInvalidData()
    {
        //// Arrange
        //var invalidProposal = new Proposal(Guid.Empty, Guid.Empty, string.Empty, 0, DateTime.UtcNow);
        //// Act & Assert
        //Assert.Throws<InvalidProposalException>(() => invalidProposal.Validate());
    }
}
