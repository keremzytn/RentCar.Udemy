using RentCarServer.Domain.Users;
using RentCarServer.Infrastructure.Context;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace RentCarServer.Infrastructure.Repositories;
internal sealed class UserRepository : Repository<User, ApplicationDbContext>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> FirstOrDefaultAsync(Func<User, bool> predicate, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_context.Set<User>().FirstOrDefault(predicate));
    }

    public async Task<User> FirstOrDefaultAsync(Func<User, bool> value)
    {
        var user = await Task.FromResult(_context.Set<User>().FirstOrDefault(value));
        if (user is null)
            throw new InvalidOperationException("User not found");
        return user;
    }

    public async Task<bool> AnyAsync(Func<User, bool> predicate)
    {
        return await Task.FromResult(_context.Set<User>().Any(predicate));
    }

    public override void Add(User user)
    {
        _context.Set<User>().Add(user);
    }
}