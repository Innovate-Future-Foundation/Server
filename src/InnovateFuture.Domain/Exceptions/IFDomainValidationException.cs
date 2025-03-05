namespace InnovateFuture.Domain.Exceptions;

public class IFDomainValidationException: Exception
{
    public IFDomainValidationException(string message) : base(message)
    {
    }
}