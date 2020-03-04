
using BO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    /// <summary>
    /// Class Base xử lý chung việc truy vấn với CSDL
    /// </summary>
    /// Created by: NVCUONG (29/04/2018)
    public class DLBase
    {
        /// <summary>
        /// Dùng để test luồng của cấu trúc
        /// </summary>
        /// <returns></returns>
        /// Created by: nvcuong
        public object GetData()
        {
            object result;
            using (IUnitOfWork _unitOfWork = new UnitOfWork())
            {
                using (MySqlConnection sqlConnection = _unitOfWork.DataContext.Connection)
                {
                    using (MySqlCommand sqlCommand = sqlConnection.CreateCommand())
                    {
                        sqlCommand.CommandType = CommandType.Text;
                        sqlCommand.CommandText = "Select ID From Users LIMIT 1";
                        sqlCommand.Transaction = _unitOfWork.BeginTransaction();
                        result = sqlCommand.ExecuteScalar();
                        _unitOfWork.Commit();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Thực thi một store với các tham số truyền vào
        /// </summary>
        /// <param name="storeName">Tên store cần thực thi</param>
        /// <param name="param">mảng các tham số truyền vào</param>
        /// <returns></returns>
        /// Created by: NVCUONG (19/04/2018)
        public int ExecuteNonQuery(string storeName, object[] param)
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

        /// <summary>
        /// Lấy dữ liệu phân trang
        /// </summary>
        /// <typeparam name="T">Đối tượng</typeparam>
        /// <param name="storeName">Tên Store sẽ lấy dữ liệu</param>
        /// <param name="paramValue">Mảng tham số truyền vào theo thứ tự</param>
        /// <returns>Bảng dữ liệu đã được phân trang</returns>
        /// Created by: NVCUONG (21/06/2018)
        public virtual Paging<T> SelectEntitiesPaging<T>(string storeName, object[] paramValue)
        {
            Paging<T> pagingEntities = new Paging<T>();
            List<T> entities = new List<T>();
            using (IUnitOfWork _unitOfWork = new UnitOfWork())
            {
                using (MySqlConnection _conn = _unitOfWork.DataContext.Connection)
                {
                    using (MySqlCommand command = _conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = storeName;
                        //command.Transaction = _unitOfWork.BeginTransaction();
                        //Gán giá trị tham số:
                        MySqlCommandBuilder.DeriveParameters(command);

                        foreach (MySqlParameter para in command.Parameters)
                        {
                            var i = command.Parameters.IndexOf(para);
                            if (i > 0 && i <= paramValue.Length)
                            {
                                para.Value = paramValue[i - 1];
                            }
                            else if (i > paramValue.Length)
                            {
                                break;
                            }
                        }

                        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                        DataSet dataset = new DataSet();

                        adapter.Fill(dataset);
                        if (dataset.Tables.Count > 0)
                        {
                            DataTable tableEntities = dataset.Tables[0];
                            DataTable tableTotalPage = dataset.Tables[1];
                            DataTable tableTotalRecord = dataset.Tables[2];

                            foreach (DataRow row in tableEntities.Rows)
                            {
                                var entity = Activator.CreateInstance<T>();
                                foreach (DataColumn column in tableEntities.Columns)
                                {
                                    string columnName = column.ColumnName;
                                    //var abc = row[column];
                                    if (entity.GetType().GetProperty(columnName) != null && row[columnName] != DBNull.Value)
                                    {
                                        entity.GetType().GetProperty(columnName).SetValue(entity, row[columnName], null);
                                    }
                                }
                                entities.Add(entity);
                            }

                            int totalRecord = (int)tableTotalRecord.Rows[0].ItemArray[0];
                            int totalPage = (int)tableTotalPage.Rows[0].ItemArray[0];
                            pagingEntities = new Paging<T>() { Entities = entities, TotalPage = totalPage, TotalRecord = totalRecord };
                        }
                    }
                }
                //_unitOfWork.Commit();
            }
            return pagingEntities;
        }
        //-----------------------------------hàm lấy thông tin phân trang dựa theo điều kiện filter
        /// <summary>
        /// hàm dùng chung cho tất cả các trang dùng để lấy dữ liệu phân trang mới nhất 
        /// </summary>
        /// <typeparam name="T">model cần lấy dữ liệu</typeparam>
        /// <param name="storeName">tên procedure</param>
        /// <param name="paramValue"> danh sách tham số cần truyền</param>
        /// <returns></returns>
        public virtual Paging<T> CommonEntitiesPaging<T>(string storeName, object[] paramValue)
        {
            Paging<T> pagingEntities = new Paging<T>();
            List<T> entities = new List<T>();
            using (IUnitOfWork _unitOfWork = new UnitOfWork())
            {
                using (MySqlConnection _conn = _unitOfWork.DataContext.Connection)
                {
                    using (MySqlCommand command = _conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = storeName;
                        command.Transaction = _unitOfWork.BeginTransaction();
                        //Gán giá trị tham số:
                        //lấy dữ liệu theo tiêu chí nào? truyền tham số vào
                        //sử dụng function AddWithValue để gán biến vào tham số trong proceduce
                        command.Parameters.AddWithValue("@CurrentPage", paramValue[0]);
                        command.Parameters.AddWithValue("@PageSize", paramValue[1]);
                        //ParameterDirection where được build động từ model->controller
                        command.Parameters.AddWithValue("@Where", paramValue[2]);
                        //khai báo params output
                        //sử dụng function Add để khởi tạo và thêm kiểu dữ liệu cho biến output
                        command.Parameters.Add("@TotalRecord", MySqlDbType.Int32);
                        command.Parameters["@TotalRecord"].Direction = ParameterDirection.Output;// output=2
                        command.Parameters.Add("@TotalPage", MySqlDbType.Int32);
                        command.Parameters["@TotalPage"].Direction = ParameterDirection.Output;//output=2


                        //thực hiện lấy dữ liệu-> sử dụng ADO
                        using (MySqlDataReader sqlDataReader = command.ExecuteReader())
                        {
                            // lấy dữ liệu dataReader từ database dùng dataReader.read() để đọc

                            while (sqlDataReader.Read())
                            {
                                var entity = Activator.CreateInstance<T>();
                                //duyệt qua từng bản ghi có trong dataReader, .fieldCount đếm số bản ghi
                                for (int i = 0; i < sqlDataReader.FieldCount; i++)
                                {
                                    //tách lấy tên các cột dữ liệu
                                    string fieldName = sqlDataReader.GetName(i);
                                    //lấy giá trị tương ứng với cột thứ i (hiện tại đang là 1 table cell)
                                    var fieldValue = sqlDataReader.GetValue(i);
                                    //để chắc chắn kiểm tra xem object trong model có tên tương ứng với tên cột trong DB hay không
                                    //gán giá trị đọc được vào các đối tượng menuGroup
                                    if (entity.GetType().GetProperty(fieldName) != null && fieldValue != DBNull.Value)
                                    {
                                        entity.GetType().GetProperty(fieldName).SetValue(entity, fieldValue);
                                    }
                                }
                                entities.Add(entity);// gán giá trị vào mảng đối tượng

                            }
                        }
                        int totalRecord = Convert.ToInt32(command.Parameters["@TotalRecord"].Value);
                        int totalPage = Convert.ToInt32(command.Parameters["@TotalPage"].Value);
                        pagingEntities.Entities = entities;
                        pagingEntities.TotalPage = totalPage;
                        pagingEntities.TotalRecord = totalRecord;
                        _unitOfWork.Commit();
                    }
                }
            }
            return pagingEntities;
        }
        /// <summary>
        /// Lấy một mảng dữ liệu cho Entity
        /// </summary>
        /// <param name="storeName">Tên store procedure</param>
        /// <returns>Mãng dữ liệu kiểu Entity</returns>
        /// Created by: NVCUONG (13/04/2018)
        public IEnumerable<T> GetEntities<T>(string storeName)
        {
            using (IUnitOfWork _unitOfWork = new UnitOfWork())
            {
                using (MySqlConnection _conn = _unitOfWork.DataContext.Connection)
                {
                    using (MySqlCommand command = _conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = storeName;
                        command.Transaction = _unitOfWork.BeginTransaction();
                        using (var sqlDataReader = command.ExecuteReader())
                        {
                            while (sqlDataReader.Read())
                            {
                                var entity = Activator.CreateInstance<T>();
                                for (int i = 0; i < sqlDataReader.FieldCount; i++)
                                {
                                    string fieldName = sqlDataReader.GetName(i);
                                    PropertyInfo property = entity.GetType().GetProperty(fieldName);
                                    if (property != null && sqlDataReader[fieldName] != DBNull.Value)
                                    {
                                        property.SetValue(entity, sqlDataReader[fieldName], null);
                                    }
                                }
                                yield return entity;
                            }
                        }
                        _unitOfWork.Commit();
                    }
                }
            }
        }


        /// <summary>
        /// Lấy mảng dữ liệu Entity
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="storeName"> Tên store</param>
        /// <param name="paramValue">mảng các tham số truyền vào</param>
        /// <returns>List Entity</returns>
        /// Created By: nvcuong (30/06/2015)
        protected virtual IEnumerable<T> GetEntities<T>(string storeName, object[] paramValue)
        {
            using (IUnitOfWork _unitOfWork = new UnitOfWork())
            {
                using (MySqlConnection _conn = _unitOfWork.DataContext.Connection)
                {
                    using (var command = _unitOfWork.DataContext.Connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = storeName;
                        command.Transaction = _unitOfWork.BeginTransaction();
                        //Gán giá trị các tham số đầu vào cho store:
                        MySqlCommandBuilder.DeriveParameters(command);
                        foreach (MySqlParameter p in command.Parameters)
                        {
                            var i = command.Parameters.IndexOf(p);
                            if (i > 0 && i <= paramValue.Length)
                            {
                                p.Value = paramValue[i - 1];
                            }
                            else if (i > paramValue.Length)
                            {
                                break;
                            }
                        }
                        MySqlDataReader sqlDataReader = command.ExecuteReader();

                        while (sqlDataReader.Read())
                        {
                            var entity = Activator.CreateInstance<T>();
                            for (int i = 0; i < sqlDataReader.FieldCount; i++)
                            {
                                string fieldName = sqlDataReader.GetName(i);
                                if (entity.GetType().GetProperty(fieldName) != null && sqlDataReader[fieldName] != DBNull.Value)
                                {
                                    entity.GetType().GetProperty(fieldName).SetValue(entity, sqlDataReader[fieldName], null);
                                }
                            }
                            yield return entity;
                        }

                    }
                }
                //_unitOfWork.Commit();
            }
        }

        /// <summary>
        /// Lấy danh sách đối tượng theo trang
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="sqlTotal"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="total"></param>
        /// Createby: nvcuong
        /// Createdate: 30/06/2015
        public IEnumerable<T> GetEntitiesPaging<T>(string storeName, string sqlTotal, int start, int limit, out int total)
        {
            List<T> lstObject = new List<T>();
            total = 0;
            using (IUnitOfWork _unitOfWork = new UnitOfWork())
            {
                using (MySqlConnection sqlConnection = _unitOfWork.DataContext.Connection)
                {
                    using (var cmd = sqlConnection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@sort", "");
                        cmd.Parameters.AddWithValue("@start", start);
                        cmd.Parameters.AddWithValue("@limit", limit);
                        cmd.Parameters.Add("@outValue", MySqlDbType.Int32);
                        cmd.Parameters["@outValue"].Direction = ParameterDirection.Output;
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            var entity = Activator.CreateInstance<T>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string fieldName = reader.GetName(i);
                                if (entity.GetType().GetProperty(fieldName) != null && reader[fieldName] != DBNull.Value)
                                {
                                    entity.GetType().GetProperty(fieldName).SetValue(entity, reader[fieldName], null);
                                }
                            }
                            lstObject.Add(entity);
                        }
                        //conn.Close();

                        //conn.Open();
                        MySqlCommand cmd2 = new MySqlCommand(sqlTotal, sqlConnection);
                        total = (int)cmd2.ExecuteScalar();
                    }
                }
            }
            //conn.Close();
            return lstObject;
        }


        /// <summary>
        /// Thêm, sửa Entity (tùy biến Store truyền vào)
        /// </summary>
        /// <typeparam name="T">entity</typeparam>
        /// <typeparam name="storeName">Tên store thực hiện cập nhật dữ liệu</typeparam>
        /// Người tạo: nvcuong
        /// Ngày tạo: 30/06/2015
        /// <returns>Số lượng bản ghi thêm/sửa thành công</returns>
        public int Update<T>(T entity, string storeName)
        {
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
                        foreach (MySqlParameter item in sqlCommand.Parameters)
                        {
                            string parameterName = item.ParameterName.Replace("@", string.Empty);

                            if (entity.GetType().GetProperty(parameterName) != null)
                            {
                                var parameterValue = entity.GetType().GetProperty(parameterName).GetValue(entity, null);
                                item.Value = (parameterValue != null ? parameterValue : DBNull.Value);
                            }
                            else
                            {
                                item.Value = DBNull.Value;
                            }
                        }
                        var result = sqlCommand.ExecuteNonQuery();
                        _unitOfWork.Commit();
                        return result;
                    }
                }
            }
        }

        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        /// <param name="storeName">tên store thực hiện xóa dữ liệu</param>
        /// <param name="nameParameters">mảng bao gồm TÊN các tham số truyền vào cho store (theo thứ tự trong store)</param>
        /// <param name="valueParameters">mảng bao gồm GIÁ TRỊ các tham số truyền vào cho store (theo thứ tự trong store)</param>
        /// <param name="numberParameter">số lượng tham số</param>
        /// Create by: nvcuong
        /// Create date: 30/06/2015
        /// <returns></returns>
        public virtual int Delete(string storeName, string[] nameParameters, object[] valueParameters, int numberParameter)
        {
            var result = 0;
            using (IUnitOfWork _unitOfWork = new UnitOfWork())
            {
                using (MySqlConnection sqlConnection = _unitOfWork.DataContext.Connection)
                {
                    using (var sqlCommand = sqlConnection.CreateCommand())
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.CommandText = storeName;
                        sqlCommand.Transaction = _unitOfWork.BeginTransaction();
                        for (int i = 0; i < numberParameter; i++)
                        {
                            sqlCommand.Parameters.AddWithValue(nameParameters[i], valueParameters[i]);
                        }
                        result = sqlCommand.ExecuteNonQuery();
                        _unitOfWork.Commit();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Xóa dữ liệu với nhiều tham số đầu vào
        /// </summary>
        /// <param name="sql">Tên store</param>
        /// <param name="param">Mảng các tham số truyền vào</param>
        /// <returns></returns>
        /// Created by: NVCUONG (19/04/2018)
        public virtual int Delete(string storeName, object[] param)
        {
            var result = 0;
            using (IUnitOfWork _unitOfWork = new UnitOfWork())
            {
                using (MySqlConnection sqlConnection = _unitOfWork.DataContext.Connection)
                {
                    using (var sqlCommand = sqlConnection.CreateCommand())
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.CommandText = storeName;
                        sqlCommand.Transaction = _unitOfWork.BeginTransaction();
                        MySqlCommandBuilder.DeriveParameters(sqlCommand);

                        int countParameters = sqlCommand.Parameters.Count - 1; // Bỏ qua param @RETURN_VALUE của các store
                        if (param.Length >= countParameters)
                        {
                            for (int i = 1; i <= countParameters; i++)
                            {
                                sqlCommand.Parameters[i].Value = param[i - 1];
                            }
                        }
                        else
                        {
                            throw new System.ArgumentException("Tham số đầu vào cho store không đủ", "Error");
                        }
                        result = sqlCommand.ExecuteNonQuery();
                        _unitOfWork.Commit();
                    }
                }
            }
            return result;
        }
    }
}