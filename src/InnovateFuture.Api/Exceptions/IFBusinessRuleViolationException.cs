namespace InnovateFuture.Api.Exceptions;


public class IFBusinessRuleViolationException:Exception
{
    public IFBusinessRuleViolationException(string message):base(message)
    {}
}