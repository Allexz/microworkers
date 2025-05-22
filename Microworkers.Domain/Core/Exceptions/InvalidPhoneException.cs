namespace Microworkers.Domain.Core.Exceptions;
public class InvalidPhoneException : Exception
{
    private readonly string Property;
    public InvalidPhoneException(string message)
        :base(message) { }

    public InvalidPhoneException(string message, string property)
        :base(message) {  Property = property; }

}
