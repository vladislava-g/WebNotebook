using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNotebook.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);
        IEnumerable<T> GetAll();
        void Create(T obj);
        void Delete(T obj);
        void SaveChanges();
    }
}
