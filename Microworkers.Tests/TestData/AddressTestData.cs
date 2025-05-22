using Bogus;
using Bogus.DataSets;
using VO = Microworkers.Domain.Core.ValueObjects;
namespace Microworkers.Tests.TestData;
public static class AddressTestData
{
    public static VO.Address GenerateValidAddress()
    {
        var faker = new Faker("pt_BR");
        string state = faker.Address.State();
        string city = faker.Address.City();

        return VO.Address.Create(
            state: state.Length > 2
            ? state.Substring(0, 2)
            : state,
            zipCode: faker.Address.ZipCode("#####-###"),
            city: city.Length > 75
            ? city.Substring(0, 75)
            : city,
            neighborHood: "Sta Monica",
            street: faker.Address.StreetName(),
            number: faker.Random.Number(100, 200).ToString(),
            additional: "Apto 101"
        );
    }
}
