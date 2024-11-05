using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ToDoList.Persistence.Repositories;

public interface IRepository<T> where T : class
{
    public void Create(T item);
    IEnumerable<T> Read();
    T? ReadById(int id);
    bool Update(T item);
    bool Delete(int id);
}
