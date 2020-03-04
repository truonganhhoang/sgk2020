using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class DLBasePattern
    {
        /// <summary>
        /// Thực thi một store với các tham số truyền vào
        /// </summary>

        public int ExecuteNonQuery(string query, object[] parameters, CommandType commandType=CommandType.Text)
        {
            int result;
            using (IUnitOfWork _unitOfWork = new UnitOfWork())
            {
                using (MySqlConnection sqlConnection = _unitOfWork.DataContext.Connection)
                {
                    using (MySqlCommand sqlCommand = sqlConnection.CreateCommand())
                    {
                        //Gán giá trị tham số:
                        for (int i = 1; i < parameters.Length; ++i)
                        {
                            // do smt
                        }
                        sqlCommand.CommandType = commandType;
                        sqlCommand.CommandText = query;
                        sqlCommand.Transaction = _unitOfWork.BeginTransaction();
                        
                        result = sqlCommand.ExecuteNonQuery();
                        _unitOfWork.Commit();
                    }
                }
            }
            return result;
        }

        public int GetNumberOfRecord(string storeName, object[] param)
        {
            int result;
            using (IUnitOfWork _unitOfWork = new UnitOfWork())
            {
                using (MySqlConnection sqlConnection = _unitOfWork.DataContext.Connection)
                {
                    using (MySqlCommand sqlCommand = sqlConnection.CreateCommand())
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.CommandText = storeName;
                        sqlCommand.Transaction = _unitOfWork.BeginTransaction();
                        //Gán giá trị tham số:
                        MySqlCommandBuilder.DeriveParameters(sqlCommand);
                        foreach (MySqlParameter p in sqlCommand.Parameters)
                        {
                            var i = sqlCommand.Parameters.IndexOf(p);
                            if (i > 0 && i <= param.Length)
                            {
                                p.Value = param[i - 1];
                            }
                            else if (i > param.Length)
                            {
                                break;
                            }
                        }
                        result = (int)sqlCommand.ExecuteScalar();
                        _unitOfWork.Commit();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Thực thi câu lệnh sql lấy một giá trị đơn trả về
        /// </summary>
        /// <param name="storeName">tên store cần thực thi</param>
        /// <param name="param">mảng các tham số cần truyền vào</param>
        /// <returns>Giá trị đơn cần trả về (có thể là int, string,...)</returns>
        /// Created by: NVCUONG (19/04/2018)
        public object ExecuteScalar(string storeName, object[] param)
        {
            object result;
            using (IUnitOfWork unitOfWork = new UnitOfWork())
            {
                using (MySqlConnection sqlConnection = unitOfWork.DataContext.Connection)
                {
                    using (MySqlCommand sqlCommand = sqlConnection.CreateCommand())
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.CommandText = storeName;
                        sqlCommand.Transaction = unitOfWork.BeginTransaction();
                        //Gán giá trị tham số:
                        MySqlCommandBuilder.DeriveParameters(sqlCommand);
                        foreach (MySqlParameter p in sqlCommand.Parameters)
                        {
                            var i = sqlCommand.Parameters.IndexOf(p);
                            if (i > 0 && i <= param.Length)
                            {
                                p.Value = param[i - 1];
                            }
                            else if (i > param.Length)
                            {
                                break;
                            }
                        }
                        result = sqlCommand.ExecuteScalar();
                        unitOfWork.Commit();
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// Thực thi câu lệnh sql lấy về một (hoặc List) đối tượng
        /// </summary>
        /// <param name="storeName">tên store cần thực thi</param>
        /// <param name="param">mảng các tham số cần truyền vào</param>
        /// <returns>Giá trị đơn cần trả về (có thể là int, string,...)</returns>
        /// Created by: NVCUONG (19/04/2018)
        public MySqlDataReader ExecuteReader(string storeName, object[] paramValue)
        {
            MySqlDataReader sqlDataReader;
            using (IUnitOfWork _unitOfWork = new UnitOfWork())
            {
                using (MySqlConnection sqlConnection = _unitOfWork.DataContext.Connection)
                {
                    using (MySqlCommand sqlCommand = sqlConnection.CreateCommand())
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.CommandText = storeName;
                        sqlCommand.Transaction = _unitOfWork.BeginTransaction();
                        MySqlCommandBuilder.DeriveParameters(sqlCommand);
                        foreach (MySqlParameter p in sqlCommand.Parameters)
                        {
                            var i = sqlCommand.Parameters.IndexOf(p);
                            if (i > 0 && i <= paramValue.Length)
                            {
                                p.Value = paramValue[i - 1];
                            }
                            else if (i > paramValue.Length)
                            {
                                break;
                            }
                        }
                        sqlDataReader = sqlCommand.ExecuteReader();
                    }
                }
            }
            return sqlDataReader;
        }

    }
}
