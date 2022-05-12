using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ICollections.Data.Interfaces;
public interface IEntityBaseRepository<T>  where T : class
{
    void Add(T entity);
    ValueTask<EntityEntry<T>> AddAsync(T entity);
    void Delete(T entity);
    void DeleteWhere(Expression<Func<T, bool>> predicate);
    void Commit();
    ValueTask CommitAsync();
    IEnumerable<T> GetAll();
    T? Find(int id);
    Task<T?> FindAsync(int id);
    T? Find(string id);
    Task<T?> FindAsync(string id);
    IEnumerable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
    T? GetSingle(Expression<Func<T, bool>> predicate);
    ValueTask<T?> GetSingleAsync(Expression<Func<T, bool>> predicate);
    T? GetSingle(Expression<Func<T?, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
    Task<T?> GetSingleAsync(Expression<Func<T?, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
    IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
    IQueryable<T> FullTextSearch(string? searchString);
    void Update(T entity);
}