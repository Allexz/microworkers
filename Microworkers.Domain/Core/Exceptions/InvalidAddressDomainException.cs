namespace Microworkers.Domain.Core.Exceptions;
public class InvalidAddressDomainException : Exception
{
    public InvalidAddressDomainException(string message)
        :base(message) { }
}
