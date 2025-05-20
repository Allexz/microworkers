namespace Microworkers.Domain.Core.Exceptions;
public class InvalidUserDomainException : Exception
{
    public InvalidUserDomainException(string message)
        : base(message) { }
}
