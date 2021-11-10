using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IRepository<T> where T : class, new()
    {
        IEnumerable<T> GetAll(string table = null);
        void Update(T obj);
        T GetById(int ID);
        void Add(T obj);
        void Delete(T obj);
        void Save();
    }
}
