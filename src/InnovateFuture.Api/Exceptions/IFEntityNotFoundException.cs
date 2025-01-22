namespace InnovateFuture.Api.Exceptions;


public class IFEntityNotFoundException:Exception
{
    public IFEntityNotFoundException(string entityName, object key)
        : base($"{entityName} with key {key} was not found.") { }
}