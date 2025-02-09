using InnovateFuture.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace InnovateFuture.Infrastructure.Users.Persistence.Interfaces;

public interface IUserRepository
{
   
    // Generate Email Confirmation Token
    Task<User> GetByIdAsync(Guid id);
    // Get all Users
}