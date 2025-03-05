namespace InnovateFuture.Infrastructure.Exceptions;

public class IFConcurrencyException : Exception
{
    public IFConcurrencyException(string message) : base(message) { }
}