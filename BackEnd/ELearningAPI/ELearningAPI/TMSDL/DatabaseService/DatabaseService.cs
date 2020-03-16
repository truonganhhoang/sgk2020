using Dapper;
using Library;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TMSDL
{
    /// <summary>
    /// xử lý tương tác với server
    /// </summary>
    public class DatabaseService : IDatabaseService
    {
        #region get summary
        /// <summary>
        /// lấy connection
        /// </summary>
        /// <param name="appCode"></param>
        /// <returns></returns>
        public IDbConnection GetConnection(string appCode)
        {
            var connString = CommonUtility.GetConnectionString();
            return new MySqlConnection(connString);
        }
        private string _modelNamespace;
        /// <summary>
        /// lấy type model từ BL
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        private Type GetModelType(string typeName)
        {
            Type type = null;
            type = Type.GetType($"{_modelNamespace}.{typeName}, {_modelNamespace}");

            if (type == null)
            {
                throw new ArgumentException($"Type [{typeName}] not found.");
            }
            return type;
        }


        /// <summary>
        /// Sinh giá trị where dựa vào kiểu dữ liệu parameter
        /// </summary>
        /// <param name="value">Parameter</param>
        private object GetSqlValue(string value)
        {
            Guid guidValue = Guid.Empty;
            if (Guid.TryParse(value, out guidValue))
            {
                return guidValue;
            }
            else
            {
                long longValue = 0;
                if (long.TryParse(value, out longValue))
                {
                    return longValue;
                }
                else
                {
                    return value;
                }
            }
        }
        /// <summary>
        /// Tự động ghép điều kiện 
        /// </summary>
        /// <returns></returns>
        private void ProcessCondition(ref string commandText, ref Dictionary<string, object> dicParam, bool isFirstCondition = true, string alias = "")
        {
            var appendCondition = isFirstCondition ? "WHERE" : "AND";
            var appendAlias = !string.IsNullOrWhiteSpace(alias) ? (alias + ".") : string.Empty;
            commandText += $" {appendCondition} {appendAlias}";
        }
        #endregion
        #region excute method
        /// <summary>
        /// thực thi một thủ tục trả về trạng thái true hay false
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Execute(string sql, CommandType commandType = CommandType.StoredProcedure, object param = null, IDbTransaction transaction = null, IDbConnection connection = null, string appCode = "")
        {
            bool success = true;
            using (var cnn = connection ?? GetConnection(appCode))
            {
                success = await cnn.ExecuteAsync(sql, param, commandType: commandType, transaction: transaction) > 0;
            }
            return success;
        }
        /// <summary>
        /// lấy về giá trị bản ghi
        /// </summary>
        /// <param name="appCode"></param>
        /// <param name="commandText"></param>
        /// <param name="dicParam"></param>
        /// <param name="modelType"></param>
        /// <param name="transaction"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> QueryUsingCommandText(string appCode, string commandText, Dictionary<string, object> dicParam, Type modelType, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            IEnumerable<dynamic> result = null;
            var cd = new CommandDefinition(commandText, commandType: CommandType.Text, parameters: dicParam, transaction: transaction);
            var con = (transaction != null ? transaction.Connection : connection);
            if (con != null)
            {
                result = await con.QueryAsync(modelType, cd);
            }
            else
            {
                using (var cnn = GetConnection(appCode))
                {
                    result = await cnn.QueryAsync(modelType, cd);
                }
            }
            return result;
        }
        /// <summary>
        /// xử lý theo command text
        /// </summary>

        /// <param name="appCode"></param>
        /// <param name="commandText"></param>
        /// <param name="dicParam"></param>
        /// <param name="transaction"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public async Task<bool> ExecuteUsingCommandText(string appCode, string commandText, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            bool success = true;
            var cd = new CommandDefinition(commandText, commandType: CommandType.Text, parameters: dicParam, transaction: transaction);
            var con = (transaction != null ? transaction.Connection : connection);
            if (con != null)
            {
                success = await con.ExecuteAsync(cd) > 0;
            }
            else
            {
                using (var cnn = GetConnection(appCode))
                {
                    success = await cnn.ExecuteAsync(cd) > 0;
                }
            }
            return success;
        }
        // trả về kết quả là số hàng bị tác động 
        public async Task<int> ExecuteReturnRowEffect(string sql, CommandType commandType = CommandType.StoredProcedure, object param = null, IDbTransaction transaction = null, IDbConnection connection = null, string appCode = "")
        {
            int result;
            using (var cnn = connection ?? GetConnection(appCode))
            {
                result = await cnn.ExecuteAsync(sql, param, commandType: commandType, transaction: transaction);
            }
            return result;
        }
        /// 
        /// trả về kết quả là cell đầu tiên trong bảng kết quả
        public async Task<T> ExecuteScalar<T>(string sql, CommandType commandType = CommandType.StoredProcedure, object param = null, IDbTransaction transaction = null, IDbConnection connection = null, string appCode = "")
        {
            T result = default(T);
            using (var cnn = connection ?? GetConnection(appCode))
            {
                result = await cnn.ExecuteScalarAsync<T>(sql, param, commandType: commandType, transaction: transaction);
            }
            return result;
        }
        /// Viết hàm xóa ở đây

        #endregion

        #region Query method
        // trả về kết quả là danh sách object T
        public async Task<List<T>> Query<T>(string sql, CommandType commandType = CommandType.StoredProcedure, object param = null, IDbTransaction transaction = null, IDbConnection connection = null, string appCode = "")
        {
            List<T> result = new List<T>();
            using (var cnn = connection ?? GetConnection(appCode))
            {
                var query = await cnn.QueryAsync<T>(sql, param, commandType: commandType, transaction: transaction);
                result = query.ToList();
            }
            return result;
        }
        // trả về kết quả là danh sách bất kỳ, dạng dynamic
        public async Task<IEnumerable<dynamic>> Query(string sql, CommandType commandType = CommandType.StoredProcedure, object param = null, IDbTransaction transaction = null, IDbConnection connection = null, string appCode = "")
        {
            IEnumerable<dynamic> result = null;
            using (var cnn = connection ?? GetConnection(appCode))
            {
                result = await cnn.QueryAsync(sql, param, commandType: commandType, transaction: transaction);
            }
            return result;
        }
        // trả về kết quả là list danh sách với kiểu là bất kì (list of list<obj>)
        public async Task<List<List<object>>> QueryMultiple(List<Type> types, string sql, CommandType commandType = CommandType.StoredProcedure, object param = null, IDbTransaction transaction = null, IDbConnection connection = null, string appCode = "")
        {
            List<List<object>> result = new List<List<object>>();
            using (var cnn = connection ?? GetConnection(appCode))
            {
                // excute ra các kết quả DataReader
                using (var multi = cnn.QueryMultiple(sql, param, commandType: commandType, transaction: transaction))
                {
                    int index = 0;
                    do
                    {
                        // đọc lần lượt từng kết quả DataReader, thêm vào list result
                        result.Add(multi.Read(types[index]).ToList());
                    } while (!multi.IsConsumed);
                }
            }
            return result;
        }
        // trả về kết quả là dictionary key là object, value là các kết quả DataReader {Dog:[{},{}]
        public async Task<Dictionary<string, List<object>>> QueryMultiple(List<string> types, string sql, CommandType commandType = CommandType.StoredProcedure, object param = null, IDbTransaction transaction = null, IDbConnection connection = null, string appCode = "")
        {
            Dictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            List<Type> lstTypes = new List<Type>();
            for (int i = 0; i < types.Count; i++)
            {
                lstTypes.Add(GetModelType(types[i]));
            }
            var listRes = await QueryMultiple(lstTypes, sql, commandType, param, transaction, connection);
            for (int i = 0; i < types.Count; i++)
            {
                result.Add(types[i], listRes[i]);
            }
            return result;
        }
        /// <summary>
        /// xử lý store, trả về giá trị đơn (ID, count,...)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="appCode"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public async Task<T> ExecuteScalarUsingStoredProcedure<T>(string appCode, string storedProcedureName, object param = null, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            T result = default(T);
            var cd = new CommandDefinition(storedProcedureName, commandType: CommandType.StoredProcedure, parameters: param, transaction: transaction);
            var con = (transaction != null ? transaction.Connection : connection);
            if (con != null)
            {
                result = await con.ExecuteScalarAsync<T>(cd);
            }
            else
            {
                using (var cnn = GetConnection(appCode))
                {
                    result = await cnn.ExecuteScalarAsync<T>(cd);
                }
            }
            return result;
        }
        /// <summary>
        /// xóa theo khóa chính và tên bảng
        /// </summary>
        /// <param name="appCode"></param>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <param name="transaction"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public async Task<bool> ExecuteDeleteByFieldNameAndValue(string appCode, string tableName, string fieldName, object fieldValue, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            string commandText = string.Empty;
            var dicParam = new Dictionary<string, object>();
            GenerateDeleteByFieldNameAndValue(ref commandText, ref dicParam, tableName, fieldName, fieldValue);
            bool success = await ExecuteUsingCommandText(appCode, commandText, dicParam, transaction: transaction, connection: connection);
            return success;

        }
        // hàm bổ sung get type  model

        #endregion

        #region generate hàm xóa
        /// <summary>
        /// generate ra câu xóa
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="dicParam"></param>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        private void GenerateDeleteByFieldNameAndValue(ref string commandText, ref Dictionary<string, object> dicParam, string tableName, string fieldName, object fieldValue)
        {
            commandText = $"DELETE FROM `{Utils.SafeSqlLiteralForObjectName(tableName)}`";
            ProcessCondition(ref commandText, ref dicParam);

            if (fieldValue != null)
            {
                commandText = commandText + $" AND `{Utils.SafeSqlLiteralForObjectName(fieldName)}` = @ForeignKeyValue";
                dicParam.Add("@ForeignKeyValue", fieldValue);
            }
            else
            {
                throw new Exception("DELETE without ID");
            }
        }

        #endregion


        #region getByID
        /// <summary>
        /// get object bởi khóa chính(id)
        /// </summary>
        /// <param name="appCode"></param>
        /// <param name="modelType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<object> GetByID(string appCode, Type modelType, string id)
        {
            string commandText = string.Empty;
            var dicParam = new Dictionary<string, object>();
            GenerateSelectById(ref commandText, ref dicParam, modelType, id);

            var data = await QueryUsingCommandText(appCode, commandText, dicParam, modelType);

            return data.Count() > 0 ? data.First() : null;
        }
        /// <summary>
        /// gen ra câu lấy theo id
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="dicParam"></param>
        /// <param name="type"></param>
        /// <param name="id"></param>
        private void GenerateSelectById(ref string commandText, ref Dictionary<string, object> dicParam, Type type, string id)
        {
            commandText = $"SELECT t.* FROM `{Utils.SafeSqlLiteralForObjectName(type.GetViewOrTableName())}` AS t";
            ProcessCondition(ref commandText, ref dicParam);

            var idValue = GetSqlValue(id);
            if (id != null)
            {
                commandText = commandText + $" AND {type.GetPrimaryKeyFieldName()} = @IDValue";
                dicParam.Add("@IDValue", idValue);
            }
            else
            {
                throw new Exception("SelectByID without ID.");
            }
        }
        #endregion
    }
}