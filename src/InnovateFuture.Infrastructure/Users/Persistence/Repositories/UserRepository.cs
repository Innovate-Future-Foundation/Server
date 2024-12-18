using InnovateFuture.Infrastructure.Common.Persistence;
using InnovateFuture.Infrastructure.Orders.Persistence.Interfaces;

namespace InnovateFuture.Infrastructure.Users.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //CRUD
    }
}