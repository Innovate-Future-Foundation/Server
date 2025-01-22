namespace InnovateFuture.Api.Exceptions;


public class IFConcurrencyException : Exception
{
    public IFConcurrencyException(string message) : base(message) { }
}