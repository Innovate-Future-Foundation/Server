namespace InnovateFuture.Api.Exceptions;

public class IFAggregateNotInitializedException: Exception
{
    public IFAggregateNotInitializedException(string message) : base(message) { }
}