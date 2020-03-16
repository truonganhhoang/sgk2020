using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMSBO;

namespace TMSBL
{
    public interface IBaseBL
    {
        #region SaveData
        List<ValidateResult> ValidateSaveData(string appCode, BaseModel baseModel);

        /// <summary>
        /// lưu hoặc sửa một đối tượng
        /// </summary>
        /// <param name="baseModel"></param>
        /// <param name="appCode"></param>
        /// <returns></returns>
        ServiceResult SaveData(BaseModel baseModel, string appCode = "");

        /// <summary>
        /// lấy theo ID
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        dynamic GetByID(string typeName, string id);

        #endregion

        #region Excecute Method
        /// <summary>
        /// thực thi một câu lệnh, trả về trạng thái true or false
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="connection"></param>
        /// <param name="appCode"></param>
        Task<bool> Execute(string sql, CommandType commandType = CommandType.StoredProcedure, object param = null, IDbTransaction transaction = null, IDbConnection connection = null, string appCode = "");

        /// <summary>
        /// hàm xử lý trả về một giá trị đơn, có thể là 1 object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="connection"></param>
        /// <param name="appCode"></param>
        /// <returns></returns>
        Task<T> ExecuteScalar<T>(string sql, CommandType commandType = CommandType.StoredProcedure, object param = null, IDbTransaction transaction = null, IDbConnection connection = null, string appCode = "");

        /// <summary>
        /// xử lý store, trả về giá trị duy nhất có kiểu là T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedureName"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        T ExecuteScalarUsingStoredProcedure<T>(string storedProcedureName, object param = null, IDbTransaction transaction = null, IDbConnection connection = null);

        #endregion
        #region QueryMethod
        /// <summary>
        ///  trả về kết quả là danh sách object T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="connection"></param>
        /// <param name="appCode"></param>
        /// <returns></returns>
        Task<List<T>> Query<T>(string sql, CommandType commandType = CommandType.StoredProcedure, object param = null, IDbTransaction transaction = null, IDbConnection connection = null, string appCode = "");

        /// <summary>
        /// trả về kết quả là danh sách bất kỳ, dạng dynamic
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="connection"></param>
        /// <param name="appCode"></param>
        /// <returns></returns>
        Task<IEnumerable<dynamic>> Query(string sql, CommandType commandType = CommandType.StoredProcedure, object param = null, IDbTransaction transaction = null, IDbConnection connection = null, string appCode = "");

        /// <summary>
        /// trả về kết quả là list danh sách với kiểu là bất kì (list of list<obj>)
        /// </summary>
        /// <param name="types"></param>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="connection"></param>
        /// <param name="appCode"></param>
        /// <returns></returns>
        Task<List<List<object>>> QueryMultiple(List<Type> types, string sql, CommandType commandType = CommandType.StoredProcedure, object param = null, IDbTransaction transaction = null, IDbConnection connection = null, string appCode = "");

        /// <summary>
        /// trả về kết quả là dictionary key là object, value là các kết quả DataReader {Dog:[{},{}]
        /// </summary>
        /// <param name="types"></param>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="connection"></param>
        /// <param name="appCode"></param>
        /// <returns></returns>
        Task<Dictionary<string, List<object>>> QueryMultiple(List<string> types, string sql, CommandType commandType = CommandType.StoredProcedure, object param = null, IDbTransaction transaction = null, IDbConnection connection = null, string appCode = "");
        #endregion
        #region delete
        /// <summary>
        /// hàm xóa đối tượng, trả về thành công hay không?
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <param name="transaction"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        bool ExecuteDeleteByFieldNameAndValue(string tableName, string fieldName, object fieldValue, IDbTransaction transaction = null, IDbConnection connection = null);

        /// <summary>
        /// Xóa dữ liệu 
        /// </summary>
        /// <param name="baseModel"></param>
        /// <returns></returns>
        ServiceResult DeleteData(BaseModel baseModel);



        #endregion
    }
}
