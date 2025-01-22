namespace InnovateFuture.Api.Exceptions;


public class IFPolicyViolationException : Exception
{
    public IFPolicyViolationException(string message) : base(message) { }
}