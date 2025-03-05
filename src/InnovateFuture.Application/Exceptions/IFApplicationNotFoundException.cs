namespace InnovateFuture.Application.Exceptions;

public class IFApplicationNotFoundException: Exception
{
    public IFApplicationNotFoundException(string message):base($"Application not found error: {message}"){}
}