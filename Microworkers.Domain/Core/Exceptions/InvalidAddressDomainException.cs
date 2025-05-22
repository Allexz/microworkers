namespace Microworkers.Domain.Core.Exceptions;
public class InvalidAddressDomainException : Exception
{
    private readonly string Property;
    public InvalidAddressDomainException(string message)
        :base(message) { }

    public InvalidAddressDomainException(string message, string property)
        : base(message) 
    {
        Property = property;
    }

    
}
