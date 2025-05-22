namespace Microworkers.Domain.Core.ValueObjects;

public record Address
{
    private Address() { }

    internal Address(string state, string zipCode, string city, string neighborHood, string street, string number, string? additional)
    {
        State = state;
        ZipCode = zipCode;
        City = city;
        NeighborHood = neighborHood;
        Street = street;
        Number = number;
        Additional = additional;
    }
    public string State { get; init; }
    public string ZipCode { get; init; }
    public string City { get; init; }
    public string NeighborHood { get; init; }
    public string Street { get; init; }
    public string Number { get; init; }
    public string? Additional { get; init; } 

}