using Library;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TMSBO;
using TMSDL;

namespace TMSBL
{
    public class BaseBL: IBaseBL
    {
        private readonly string _userName = "admin";
        public string Proc_Insert = "Proc_{0}_Insert";
        public string Proc_Update = "Proc_{0}_Update";
        public string Proc_Delete = "Proc_{0}_Delete";
        protected string ApplicationCode;
        protected string _modelNamespace;
        protected DatabaseService DL;
        protected string AppCode = "E_Learning";
        public BaseBL()
        {
            this.DL = new DatabaseService();
        }
        #region SaveData
        public virtual List<ValidateResult> ValidateSaveData(string appCode, BaseModel baseModel)
        {
            List<ValidateResult> results = new List<ValidateResult>();
            return results;
        }
        /// <summary>
        /// trước khi lưu thêm một số tham số trong base
        /// </summary>
        /// <param name="oEntity"></param>
        /// <param name="appCode"></param>
        public virtual void BeforeSave(ref BaseModel oEntity, string appCode = "")
        {
            // do smt
            if (oEntity.State == Library.EntityState.Insert || oEntity.State == Library.EntityState.Duplicate)
            {
                oEntity.CreatedDate = DateTime.Now;
                oEntity.CreatedBy = _userName;
            }
            oEntity.ModifiedBy = _userName;
            oEntity.ModifiedDate = DateTime.Now;
        }
        public static PropertyInfo[] GetProperties(BaseModel model, PropertyInfo[] properties = null)
        {
            if (properties == null)
            {
                properties = model.GetType().GetProperties();
            }
            else
            {

            }
            return properties;
        }
        /// <summary>
        /// thực hiện lưu/ sửa
        /// </summary>
        /// <param name="baseModel"></param>
        /// <param name="transaction"></param>
        /// <param name="appCode"></param>
        /// <returns></returns>
        private bool DoSave(BaseModel baseModel, IDbTransaction transaction, string appCode = "")
        {
            //Save Master
            string storeName = Proc_Insert;
            object primaryKeyValue = baseModel.GetPrimaryKeyValue();
            if (baseModel.State == EntityState.Update)
            {
                storeName = Proc_Update;
            }
            var dic = Utils.ConvertDatabaseParam(baseModel);
            if (baseModel.GetPrimaryKeyType() == typeof(int))
            {
                primaryKeyValue = this.ExecuteScalarUsingStoredProcedure<int>(string.Format(storeName, baseModel.GetTableName()), dic, transaction);
                if (baseModel.State == EntityState.Insert || baseModel.State == EntityState.Duplicate)
                {
                    baseModel.SetValueByAttribute(typeof(KeyAttribute), primaryKeyValue);
                }
            }
            else
            {
                this.ExecuteScalarUsingStoredProcedure<object>(string.Format(storeName, baseModel.GetTableName()), dic, transaction);
            }

            // Đệ quy qua Detail để lưu model children
            if (baseModel.ModelDetailConfigs != null)
            {
                foreach (ModelDetailConfig detailConfig in baseModel.ModelDetailConfigs.Where(c => !string.IsNullOrWhiteSpace(c.PropertyOnMasterModel)))
                {
                    IList lstDetailObject = baseModel.GetValue<IList>(detailConfig.PropertyOnMasterModel);
                    if (lstDetailObject != null)
                    {
                        foreach (BaseModel detail in lstDetailObject)
                        {
                            if (detail.State == EntityState.Insert
                                || detail.State == EntityState.Duplicate
                                || detail.State == EntityState.Update)
                            {
                                //Gán lại khóa chính cho các detail
                                detail.SetValue(detailConfig.ForeignKeyName, baseModel.GetPrimaryKeyValue());
                                if (detail.State == EntityState.Insert ||
                                    detail.State == EntityState.Duplicate)
                                {
                                    detail.CreatedBy = _userName;
                                    detail.CreatedDate = DateTime.Now;
                                    if (detail.GetPrimaryKeyValue() == null)
                                    {
                                        detail.SetAutoPrimaryKey();
                                    }
                                }
                                detail.ModifiedBy = _userName;
                                detail.ModifiedDate = DateTime.Now;
                                //Lưu giá trị detail
                                DoSave(detail, transaction);
                            }
                            else if (detail.State == EntityState.Delete)
                            {
                                //TODO nếu là xóa thì gọi đến hàm xóa 
                                DoDelete(detail, transaction);
                            }

                        }
                    }
                }
            }

            return true;
        }
        /// <summary>
        /// sau khi lưu, dùng để lưu thêm detail
        /// </summary>
        public virtual void AfterSave(BaseModel baseModel, IDbTransaction transaction, string appCode = "")
        {
            // do smt
        }

        public ServiceResult SaveData(BaseModel baseModel, string appCode = "")
        {
            ServiceResult serviceResult = new ServiceResult();
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            try
            {
                // validate dữ liệu
                var validateResults = this.ValidateSaveData(appCode, baseModel);
                if (validateResults.Count > 0)
                {
                    // nếu có ít nhất 1 lỗi
                    serviceResult.ValidateInfo.AddRange(validateResults);
                    serviceResult.Success = false;
                    return serviceResult;
                }
                // nếu ko có lỗi
                this.BeforeSave(ref baseModel, appCode);
                // tạo connection
                connection = this.DL.GetConnection(appCode);
                connection.Open();
                // tạo transaction
                transaction = connection.BeginTransaction();
                // thực hiện lưu
                var result = this.DoSave(baseModel, transaction, appCode);
                // sau khi lưu thành công, thực hiện aftersave
                if (result)
                {
                    AfterSave(baseModel, transaction, appCode);
                    transaction.Commit();
                    AfterCommit(baseModel, serviceResult);
                }
                else
                {
                    transaction.Rollback();
                    serviceResult.Success = true;
                }
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                    transaction.Dispose();
                }
                throw ex;
            }
            finally
            {
                if (transaction != null)
                {
                    transaction.Dispose();
                }
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            return serviceResult;
        }
        /// <summary>
        /// Xử lý nghiệp vụ sau khi lưu bản ghi thành công và không còn transaction
        /// </summary>
        /// <param name="baseModel"></param>
        public virtual void AfterCommit(BaseModel baseModel, ServiceResult serviceResponse)
        {
            //TODO
            serviceResponse.Data = this.GetByID(baseModel.GetType().Name, baseModel.GetPrimaryKeyValue().ToString());
        }
        /// <summary>
        /// lấy theo ID
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public dynamic GetByID(string typeName, string id)
        {
            return DL.GetByID(this.ApplicationCode, Type.GetType(string.Format(_modelNamespace, typeName)), id).Result;
        }
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
        /// <returns></returns>
        public Task<bool> Execute(string sql, CommandType commandType = CommandType.StoredProcedure, object param = null, IDbTransaction transaction = null, IDbConnection connection = null, string appCode = "")
        {
            return DL.Execute(sql, commandType, param, transaction, connection, appCode);
        }

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
        public Task<T> ExecuteScalar<T>(string sql, CommandType commandType = CommandType.StoredProcedure, object param = null, IDbTransaction transaction = null, IDbConnection connection = null, string appCode = "")
        {
            return DL.ExecuteScalar<T>(sql, commandType, param, transaction, connection, appCode);
        }


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
        public Task<List<T>> Query<T>(string sql, CommandType commandType = CommandType.StoredProcedure, object param = null, IDbTransaction transaction = null, IDbConnection connection = null, string appCode = "")
        {
            return DL.Query<T>(sql, commandType, param, transaction, connection, appCode);
        }
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
        public Task<IEnumerable<dynamic>> Query(string sql, CommandType commandType = CommandType.StoredProcedure, object param = null, IDbTransaction transaction = null, IDbConnection connection = null, string appCode = "")
        {
            return DL.Query(sql, commandType, param, transaction, connection, appCode);
        }
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
        public Task<List<List<object>>> QueryMultiple(List<Type> types, string sql, CommandType commandType = CommandType.StoredProcedure, object param = null, IDbTransaction transaction = null, IDbConnection connection = null, string appCode = "")
        {
            return DL.QueryMultiple(types, sql, commandType, param, transaction, connection, appCode);
        }
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
        public Task<Dictionary<string, List<object>>> QueryMultiple(List<string> types, string sql, CommandType commandType = CommandType.StoredProcedure, object param = null, IDbTransaction transaction = null, IDbConnection connection = null, string appCode = "")
        {
            return DL.QueryMultiple(types, sql, commandType, param, transaction, connection, appCode);
        }

        #endregion
        /// <summary>
        /// xử lý store, trả về giá trị duy nhất có kiểu là T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedureName"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public T ExecuteScalarUsingStoredProcedure<T>(string storedProcedureName, object param = null, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            return DL.ExecuteScalarUsingStoredProcedure<T>(this.ApplicationCode, storedProcedureName, param, transaction, connection).Result;
        }
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
        public bool ExecuteDeleteByFieldNameAndValue(string tableName, string fieldName, object fieldValue, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            return DL.ExecuteDeleteByFieldNameAndValue(this.ApplicationCode, tableName, fieldName, fieldValue, transaction, connection).Result;
        }
        /// <summary>
        /// xóa đệ quy
        /// </summary>
        /// <param name="baseModel"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual bool DoDelete(BaseModel baseModel, IDbTransaction transaction)
        {
            //Kiểm tra có bảng liên detail không nếu có bảng detail thì xóa trước khi xóa master
            var masterID = baseModel.GetPrimaryKeyValue();
            if (baseModel.ModelDetailConfigs != null)
            {
                foreach (ModelDetailConfig detailConfig in baseModel.ModelDetailConfigs.Where(c => c.CascadeOnDeleteMasterModel))
                {
                    this.ExecuteDeleteByFieldNameAndValue(detailConfig.DetailTableName, detailConfig.ForeignKeyName, masterID, transaction: transaction);
                }
            }
            this.ExecuteDeleteByFieldNameAndValue(baseModel.GetTableName(), baseModel.GetPrimaryKeyFieldName(), masterID, transaction);
            return true;
        }
        /// <summary>
        /// Chuẩn bị dữ liệu trước khi xóa
        /// </summary>
        /// <param name="baseModel">Data</param>
        public virtual void BeforeDelete(BaseModel baseModel)
        {

        }
        /// <summary>
        /// Kiểm tra dữ liệu xóa có hợp lệ hay không
        /// </summary>
        /// <param name="baseModel">Data</param>
        /// <returns></returns>
        public virtual List<ValidateResult> ValidateDeleteData(BaseModel baseModel)
        {
            var validateResults = new List<ValidateResult>();
            return validateResults;
        }
            /// <summary>
            /// Xóa dữ liệu 
            /// </summary>
            /// <param name="baseModel"></param>
            /// <returns></returns>
            public ServiceResult DeleteData(BaseModel baseModel)
        {
            ServiceResult serviceResponse = new ServiceResult();
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            try
            {
                //Xử lý dữ liệu trước khi xóa 
                BeforeDelete(baseModel);
                //Xử lý validate dữ liệu trước khi xóa
                var validateResults = ValidateDeleteData(baseModel);
                if (validateResults.Count > 0)
                {
                    serviceResponse.ValidateInfo = validateResults;
                    serviceResponse.Success = false;
                    return serviceResponse;
                }
                //Thực hiện xóa dữ liệu
                connection = DL.GetConnection(this.AppCode);
                connection.Open();
                transaction = connection.BeginTransaction();
                var res = DoDelete(baseModel, transaction);
                //Xử lý sau khi Xóa thành công  cùng transaction
                if (res)
                {
                    AfterDelete(baseModel, serviceResponse, transaction);
                    //Commit transaction
                    transaction.Commit();
                    //Xử lý sau khi xóa không còn transaction
                    AfterDeleteCommit(baseModel, serviceResponse);
                }
                else
                {
                    transaction.Rollback();
                    serviceResponse.Success = false;
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                throw ex;
            }
            finally
            {
                if (transaction != null)
                {
                    transaction.Dispose();
                }
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            return serviceResponse;
        }
        /// <summary>
        /// Xử lý sau khi xóa dữ liệu vẫn còn transanction
        /// </summary>
        /// <param name="baseModel">Data</param>
        /// <param name="serviceResponse">Kết quả trả về</param>
        public virtual void AfterDelete(BaseModel baseModel, ServiceResult serviceResponse, IDbTransaction transaction)
        {

        }
        /// <summary>
        /// Thực hiện sau khi xóa xong và close transaction
        /// </summary>
        /// <param name="baseModel"></param>
        /// <param name="serviceResponse"></param>
        public virtual void AfterDeleteCommit(BaseModel baseModel, ServiceResult serviceResponse)
        {

        }
        #endregion
    }
}