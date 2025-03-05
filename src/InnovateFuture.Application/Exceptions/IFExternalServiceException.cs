namespace InnovateFuture.Application.Exceptions;

public class IFExternalServiceException: Exception
{
    public IFExternalServiceException(string message):base($"External service error: {message}"){}
}