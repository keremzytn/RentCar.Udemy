using RentCarServer.Domain.Users;
using RentCarServer.Infrastructure.Context;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace RentCarServer.Infrastructure.Repositories;
internal sealed class UserRepository : Repository<User, ApplicationDbContext>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> FirstOrDefaultAsync(Func<User, bool> predicate)
    {
        return await Task.FromResult(_context.Set<User>().FirstOrDefault(predicate));
    }

    public async Task<bool> AnyAsync(Func<User, bool> predicate)
    {
        return await Task.FromResult(_context.Set<User>().Any(predicate));
    }
}