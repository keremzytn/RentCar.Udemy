using System;
using System.Threading.Tasks;

namespace RentCarServer.Domain.Users;

public interface IUserRepository
{
    Task<User?> FirstOrDefaultAsync(Func<User, bool> predicate);
    Task<bool> AnyAsync(Func<User, bool> predicate);
    void Add(User user);
}