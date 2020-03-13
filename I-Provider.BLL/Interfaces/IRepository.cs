using System.Collections.Generic;

namespace I_Provider.BLL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Add(T item);
        void Update(T item);
        void Delete(int id);
        void Save();
    }
}
