using Bogus;
using Microworkers.Domain.Core.Aggregates;
using Microworkers.Domain.Core.Factories;
using Microworkers.Domain.Core.ValueObjects;

namespace Microworkers.Tests.TestData;
public static class ProposalsTestData
{
    public static Proposal GenerateValidProposal()
    {
        Faker faker = new Faker("pt_BR");
        return ProposalFactory.Create(
            task: TaskiTestData.GenerateValidTaski(),
            serviceProvider: Guid.NewGuid(),
            jobDescription: ProposalDescription.Create(faker.Random.Words(10)).Value,
            price: faker.Finance.Amount(100, 1000),
            projectedDate: DateTime.UtcNow.AddDays(faker.Random.Int(1, 30))
        ).Value;
    }

    public static Proposal GenerateInvalidProposalWithEmptydescription()
    {
        Faker faker = new Faker("pt_BR");
        return ProposalFactory.Create(
            task: TaskiTestData.GenerateValidTaski(),
            serviceProvider: Guid.NewGuid(),
            jobDescription: ProposalDescription.Create(string.Empty).Value,
            price: faker.Finance.Amount(100, 1000),
            projectedDate: DateTime.UtcNow.AddDays(faker.Random.Int(1, 30))
        ).Value;
    }

    public static Proposal GenerateInvalidProposalWithLongdescription()
    {
        Faker faker = new Faker("pt_BR");
        return ProposalFactory.Create(
            task: TaskiTestData.GenerateValidTaski(),
            serviceProvider: Guid.NewGuid(),
            jobDescription: ProposalDescription.Create(faker.Random.Words(501)).Value,
            price: faker.Finance.Amount(100, 1000),
            projectedDate: DateTime.UtcNow.AddDays(faker.Random.Int(1, 30))
        ).Value;
    }

    public static Proposal GenerateInvalidProposalWithoutTask()
    {
        Faker faker = new Faker("pt_BR");
        return ProposalFactory.Create(
            task: null,
            serviceProvider: Guid.NewGuid(),
            jobDescription: ProposalDescription.Create(faker.Random.Words(501)).Value,
            price: faker.Finance.Amount(100, 1000),
            projectedDate: DateTime.UtcNow.AddDays(faker.Random.Int(1, 30))
        ).Value;
    }

    public static Proposal GenerateInvalidProposalWithoutServiceProvider()
    {
        Faker faker = new Faker("pt_BR");
        return ProposalFactory.Create(
            task: TaskiTestData.GenerateValidTaski(),
            serviceProvider: Guid.Empty,
            jobDescription: ProposalDescription.Create(faker.Random.Words(501)).Value,
            price: faker.Finance.Amount(100, 1000),
            projectedDate: DateTime.UtcNow.AddDays(faker.Random.Int(1, 30))
        ).Value;
    }

    public static Proposal GenerateInvalidProposalWithInvalidPrice()
    {
        Faker faker = new Faker("pt_BR");
        return ProposalFactory.Create(
            task: TaskiTestData.GenerateValidTaski(),
            serviceProvider: Guid.NewGuid(),
            jobDescription: ProposalDescription.Create(faker.Random.Words(501)).Value,
            price: 0,
            projectedDate: DateTime.UtcNow.AddDays(faker.Random.Int(1, 30))
        ).Value;
    }
}
