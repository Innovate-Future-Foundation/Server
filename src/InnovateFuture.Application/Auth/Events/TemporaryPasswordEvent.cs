namespace InnovateFuture.Application.Auth.Events;

public record TemporaryPasswordEvent(string UserEmail, string TemporaryPassword);