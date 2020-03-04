using BO;
using System.Collections.Generic;

namespace BL
{
    public interface IBLEntity<T>:IBL
        where T :BaseEntity
    {
        int Create(T entity);
        void Delete(T entity);
        IEnumerable<T> GetAll();    
        T GetEntityById(object id);
        int Update(T entity);
        Paging<T> SelectEntitiesPaging(object[] param);
        object GetData();
    }
}
