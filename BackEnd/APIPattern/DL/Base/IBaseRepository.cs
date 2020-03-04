using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public interface IBaseRepository<T> where T : class
    {
        int ExecuteNonQuery(string storeName, object[] param);
        int Insert(T entity);
        int Update(T entity);
        int Delete();
        IEnumerable<T> GetEntitiesAll();
        IEnumerable<T> GetEntities(string strProc);
        IEnumerable<T> GetEntities(string storeName, object[] value);
        Paging<T> SelectEntitiesPaging(object[] param);
        T GetEntityById(object id);
        int Update(T oObject, string sql);
        int Delete(string storeName, string[] name, object[] value, int Nparameter);
        object GetData();
    }
}
