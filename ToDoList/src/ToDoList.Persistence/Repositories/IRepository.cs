using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ToDoList.Persistence.Repositories;

public interface IRepositoryAsync<T> where T : class
{
    Task CreateAsync(T item);
    Task<IEnumerable<T>> ReadAllAsync();
    Task<T?> ReadByIdAsync(int id);
    Task UpdateAsync(T item);
    Task DeleteByIdAsync(int id);
    Task DeleteAllAsync();

}
