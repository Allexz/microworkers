using Bogus;
using VO = Microworkers.Domain.Core.ValueObjects;
namespace Microworkers.Tests.TestData;
public static class AddressTestData
{

    public static VO.Address GenerateValidAddress()
    {
        var faker = new Faker("pt_BR");
        return VO.Address.Create(
            state: faker.Address.StateAbbr(),
            zipCode: faker.Address.ZipCode("#####-###"),
            city: faker.Address.City(),
            neighborHood: "Sta Monica" ,
            street: faker.Address.StreetName(),
            number: faker.Random.Number(100,200).ToString(),
            additional: "Apto 101"
        );
    }
}
