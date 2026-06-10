using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);
    Task<T?> GetAsync(int id);
    Task<T?> GetAsync(int id, params Expression<Func<T, object>>[] includeProperties);
    Task CreateAsync(T item);
    void Update(T item);
    Task DeleteAsync(int id);
}
