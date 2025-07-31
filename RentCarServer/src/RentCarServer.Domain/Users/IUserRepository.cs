using System;
using System.Threading.Tasks;

namespace RentCarServer.Domain.Users;

public interface IUserRepository
{
    Task<User?> FirstOrDefaultAsync(Func<User, bool> predicate, CancellationToken cancellationToken);
    Task<bool> AnyAsync(Func<User, bool> predicate);
    void Add(User user);
    Task<User> FirstOrDefaultAsync(Func<User, bool> value);
}