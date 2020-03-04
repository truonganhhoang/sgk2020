using DL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    /// <summary>
    /// Interface kiểm soát các hàm để đảm bảo tính toàn vẹn dữ liệu
    /// </summary>
    public interface IUnitOfWork:IDisposable
    {
        IDatabaseContext DataContext { get; }
        MySqlTransaction BeginTransaction();
        void Commit();
        //void Dispose();
    }
}
