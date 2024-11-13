using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ToDoList.Persistence.Repositories;

public interface IRepository<T> where T : class
{
    void Create(T item);
    IEnumerable<T> ReadAll();
    T? ReadById(int id);
    void Update(T item);
    void DeleteById(int id);
    void DeleteAll();

}
