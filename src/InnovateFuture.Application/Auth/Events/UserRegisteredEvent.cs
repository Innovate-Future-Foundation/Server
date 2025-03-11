using InnovateFuture.Domain.Enums;

namespace InnovateFuture.Application.Auth.Events;

public record UserRegisteredEvent(string UserName, string UserEmail, Guid ProfileId, string Token, string TokenType, RoleEnum? RoleEnum = null);
    
    
    
    
    