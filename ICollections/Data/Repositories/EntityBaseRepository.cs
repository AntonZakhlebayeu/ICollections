using System.Linq.Expressions;
using ICollections.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Korzh.EasyQuery.Linq;

namespace ICollections.Data.Repositories;

public class EntityBaseRepository<T> : IEntityBaseRepository<T> where T : class
{
    private readonly CollectionDbContext _context;

    protected EntityBaseRepository(CollectionDbContext context)
    {
        _context = context;
    }

    public virtual void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }
    
    public virtual async Task<ValueTask<EntityEntry<T>>> AddAsync(T entity)
    {
        return await Task.Run(() => _context.Set<T>().AddAsync(entity));
    }
    
    public virtual void Delete(T entity)
    {
        EntityEntry dbEntityEntry = _context.Entry(entity);
        dbEntityEntry.State = EntityState.Deleted;
    }

    public virtual void DeleteWhere(Expression<Func<T, bool>> predicate)
    {
        IEnumerable<T> entities = _context.Set<T>().Where(predicate);

        foreach(var entity in entities)
        {
            _context.Entry(entity).State = EntityState.Deleted;
        }
    }

    public virtual void Commit()
    {
        _context.SaveChanges();
    }

    public virtual async Task<int> CommitAsync()
    {
        return await Task.Run(() => _context.SaveChangesAsync());
    }

    public virtual IEnumerable<T> GetAll()
    {
        return _context.Set<T>().AsEnumerable();
    }

    public virtual int Count()
    {
        return _context.Set<T>().Count();
    }
    
    public virtual IEnumerable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _context.Set<T>();
        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }
        return query.AsEnumerable();
    }

    public T? GetSingle(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().FirstOrDefault(predicate);
    }
    
    public async Task<T?> GetSingleAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(predicate);
    }

    public T? GetSingle(Expression<Func<T?, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T?> query = _context.Set<T>();
        query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty!));

        return query.Where(predicate).FirstOrDefault();
    }
    
    public async Task<T?> GetSingleAsync(Expression<Func<T?, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T?> query = _context.Set<T>();
        query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty!));

        return await query.Where(predicate).FirstOrDefaultAsync();
    }

    public virtual IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().Where(predicate);
    }

    public T? Find(int id)
    {
        return _context.Set<T>().Find(id);
    }
    
    public async Task<T?> FindAsync(int id) 
    {
        return await _context.Set<T>().FindAsync(id);
    }
    
    public T? Find(string id)
    {
        return _context.Set<T>().Find(id);
    }
    
    public async Task<T?> FindAsync(string id) 
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public virtual void Update(T entity)
    {
        EntityEntry dbEntityEntry = _context.Entry(entity);
        dbEntityEntry.State = EntityState.Modified;
    }

    public virtual IQueryable<T> FullTextSearch(string? searchString)
    {
        return _context.Set<T>().FullTextSearchQuery(searchString);
    }
}