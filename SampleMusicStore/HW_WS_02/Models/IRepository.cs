using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HW_WS_02.Models
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> All();

        T Get(int id);

        T Add(T item);

        void Delete(int id);

        T Update(int id, T item);
    }
}