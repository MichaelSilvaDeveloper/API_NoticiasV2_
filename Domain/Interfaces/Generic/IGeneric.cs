using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Generic
{
    public interface IGeneric<T> where T : class
    {
        Task Add(T Objeto);

        Task Update(T Objeto);

        Task Remove(T Objeto);

        Task<T> GetEntityById(int id);

        Task<List<T>> List();
    }
}
