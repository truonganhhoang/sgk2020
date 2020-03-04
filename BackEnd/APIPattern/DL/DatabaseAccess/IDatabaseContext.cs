using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{ 
    public interface IDatabaseContext:IDisposable
    {
        MySqlConnection Connection { get; }
    }
}
