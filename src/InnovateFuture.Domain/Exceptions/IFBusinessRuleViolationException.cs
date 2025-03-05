namespace InnovateFuture.Domain.Exceptions;

public class IFBusinessRuleViolationException:Exception
{
    public IFBusinessRuleViolationException(string message):base(message)
    {}
}