namespace InnovateFuture.Domain.Exceptions;

public class IFUnauthorizedActionException : Exception
{
    public IFUnauthorizedActionException(string message) : base(message) { }
}