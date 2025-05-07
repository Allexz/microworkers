namespace Microworkers.Domain.Core.Exceptions;
public class DomainException : Exception
{
    public DomainException(string message, string property) 
        : base(message){}
    public DomainException(string message, Exception innerException) 
        : base(message, innerException){}
}
