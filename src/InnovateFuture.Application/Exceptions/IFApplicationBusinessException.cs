namespace InnovateFuture.Application.Exceptions;

public class IFApplicationBusinessException: Exception
{
    public IFApplicationBusinessException(string message):base($"Application business error: {message}"){}
}