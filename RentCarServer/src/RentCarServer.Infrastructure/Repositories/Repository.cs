using Microsoft.EntityFrameworkCore;

namespace RentCarServer.Infrastructure.Repositories;

public class Repository<TEntity, TContext>
    where TEntity : class
    where TContext : DbContext
{
    protected readonly TContext _context;

    public Repository(TContext context)
    {
        _context = context;
    }

    public virtual void Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
    }
}