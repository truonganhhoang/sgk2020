using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;

using System.Text;
using System.Data;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Library
{
    public static class Utils
    {
        /// <summary>
        /// get giá trị của prop trong object
        /// </summary>
        /// <typeparam name="T">kiểu trả về, cũng là model mong muốn truyền vào</typeparam>
        /// <param name="objEntity">object đang xét</param>
        /// <param name="propertyName">tên thuộc tính</param>
        /// <returns></returns>
        public static T GetValue<T>(this object objEntity, string propertyName)
        {
            T value = default(T);
            // nếu object và tên prop truyền vào ko rỗng
            if (objEntity != null && !string.IsNullOrEmpty(propertyName))
            {
                // lấy thông tincuar prop
                PropertyInfo info = objEntity.GetType().GetProperty(propertyName);
                if (info != null)
                {
                    object objValue = info.GetValue(objEntity);
                    if (objValue != null)
                    {
                        value = (T)objValue;
                    }
                }
            }
            return value;
        }
        /// <summary>
        /// hàm set value cho thuộc tính vào đối tượng
        /// </summary>
        /// <param name="objEntity">trả ngược về đối tượng sau khi gán</param>
        /// <param name="propertyName">tên property</param>
        /// <param name="value">giá trị cho prop đó</param>
        public static void SetValue(this object objEntity, string propertyName, object value)
        {
            PropertyInfo propertyInfo = objEntity.GetType().GetProperty(propertyName, BindingFlags.SetProperty | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo != null)
            {
                Type type = propertyInfo.PropertyType;
                if ((!object.Equals(value, DBNull.Value)) && propertyInfo.CanWrite)
                {
                    propertyInfo.SetValue(objEntity, value, null);
                }
            }

        }
        /// <summary>
        /// hàm set param cho store mysql
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static object ConvertDatabaseParam(object o1, object o2 = null)
        {
            var result = new Dictionary<string, object>();
            AppendPropertyToDictionary(o1, ref result);
            AppendPropertyToDictionary(o2, ref result);
            return result;
        }
        /// <summary>
        /// hàm thêm prop vào dictionary trả về
        /// </summary>
        /// <param name="object1"></param>
        /// <param name="result"></param>
        public static void AppendPropertyToDictionary(object object1, ref Dictionary<string, object> result)
        {
            if (object1 != null)
            {
                if (object1.GetType() == typeof(Dictionary<string, object>))
                {
                    foreach (var item in (Dictionary<string, object>)object1)
                    {
                        result.AddOrUpdate((item.Key.StartsWith("v_") ? item.Key : ("v_" + item.Key)), item.Value);
                    }
                }
                else
                {
                    foreach(PropertyInfo pi in object1.GetType().GetProperties().Where(x=>x.CanRead && x.CanWrite && x.GetIndexParameters().Length == 0))
                    {
                        result.AddOrUpdate((pi.Name.StartsWith("v_") ? pi.Name : ("v_" + pi.Name)), pi.GetValue(object1));
                    }
                }
            }
        }
        /// <summary>
        /// thêm hoặc update 1 key có trong dic
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddOrUpdate(this Dictionary<string, object> dic, string key, object value)
        {
            // nếu tồn tại key thì là update
            if (dic.ContainsKey(key))
            {
                dic[key] = value;
            }
            else
            {
                dic.Add(key, value);
            }
        }
        /// <summary>
        /// gan tham so vao cmd trong transaction
        /// </summary>
        /// <param name="cmd">sqlcommand trong transaction</param>
        /// <param name="parameterName">ten</param>
        /// <param name="value">gia tri</param>
        public static void AddWithValue(MySqlCommand cmd, string parameterName, object value)
        {
            if (value != null)
            {
                cmd.Parameters.AddWithValue($"@{parameterName}", value);
            }
            else
            {
                cmd.Parameters.AddWithValue($"@{parameterName}", DBNull.Value);
            }
        }
        /// <summary>
        /// Lấy danh sách tham số của query để truyền dữ liệu
        /// </summary>
        /// created by: nvcuong
        public static void MappingParams(object param, MySqlCommand cmd, ref Dictionary<int, string> indexOutputParams)
        {
            if (cmd.CommandType == System.Data.CommandType.StoredProcedure)
            {
                MappingObjectWithParamsStore(param, cmd, ref indexOutputParams);
            }
            else
            {
                // tạo local variable
                Dictionary<string, object> dicParams = param as Dictionary<string, object>;
                if (dicParams != null)
                {
                    foreach (KeyValuePair<string, object> pair in dicParams)
                    {
                        AddWithValue(cmd, pair.Key, pair.Value);
                    }
                }
                else
                {
                    // get all props
                    PropertyInfo[] fs = param.GetType().GetProperties();
                    foreach (PropertyInfo f in fs)
                    {
                        string pName = f.Name;
                        object pVal = f.GetValue(param);
                        AddWithValue(cmd, pName, pVal);
                    }
                }
            }
        }
        /// <summary>
        /// mapping các giá trị với paramstore
        /// </summary>
        /// <param name="oEntity">object giá trị</param>
        /// <param name="command"></param>
        /// <param name="indexOutputParams"></param>
        public static void MappingObjectWithParamsStore(object oEntity, MySqlCommand command, ref Dictionary<int, string> indexOutputParams)
        {
            MySqlCommandBuilder.DeriveParameters(command);
            int totalParams = command.Parameters.Count;
            Type objType = oEntity.GetType();
            Dictionary<string, object> dicParams = oEntity as Dictionary<string, object>;
            for (int i = 0; i < totalParams; i++)
            {
                IDbDataParameter param = command.Parameters[i];
                string paramName = param.ParameterName.Replace("@", "");
                if (param.Direction == ParameterDirection.Output || param.Direction == ParameterDirection.InputOutput)
                {
                    indexOutputParams.Add(i, paramName);
                }
                if (dicParams != null)
                {
                    if (dicParams.ContainsKey(paramName))
                    {
                        object vVal = dicParams[paramName];
                        param.Value = vVal ?? DBNull.Value;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                }
                else
                {
                    PropertyInfo pi = objType.GetProperty(paramName);
                    if (pi != null)
                    {
                        object vVal = pi.GetValue(oEntity);
                        if (vVal != null)
                        {
                            param.Value = vVal;
                        }
                        else
                        {
                            param.Value = DBNull.Value;
                        }
                    }
                }
            }
        }
        public static void SetOutputParams(object param, MySqlCommand cmd, Dictionary<int, string> indexOutputParam)
        {
            if (param != null)
            {
                Dictionary<string, object> dictionaryParam = param as Dictionary<string, object>;
                foreach (KeyValuePair<int, string> pair in indexOutputParam)
                {
                    var outputData = cmd.Parameters[pair.Key].Value;
                    if (dictionaryParam != null)
                    {
                        dictionaryParam[pair.Value] = outputData;
                    }
                    else
                    {
                        param.SetValue(pair.Value, outputData);
                    }
                }
            }
        }
        /// ham de quy setvalue theo ten prop
        /// ham de quy setvalue theo ten prop
        /// </summary>
        /// <param name="objEntity">entity</param>
        /// <param name="propertyName">ten prop</param>
        /// <param name="value">gia tri prop</param>
        //public static void SetValue(this object objEntity, string propertyName, object value)
        //{
        //    try
        //    {
        //        // get prop by key name
        //        PropertyInfo propertyInfo = objEntity.GetType().GetProperty(propertyName);
        //        if (propertyInfo != null)
        //        {
        //            Type type = propertyInfo.PropertyType;
        //            if (!object.Equals(value, DBNull.Value) && propertyInfo.CanWrite)
        //            {
        //                // de quy set value cho entity
        //                if (propertyInfo.GetCustomAttribute<ColumnJsonAttribute>() != null)
        //                {
        //                    propertyInfo.SetValue(objEntity, JsonConvert.DeserializeObject(value.ToString(), type), null);
        //                }
        //                else
        //                {
        //                    object finalValue = value.ChangeType(type);
        //                    propertyInfo.SetValue(objEntity, finalValue, null);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // log loi
        //        PropertyInfo propertyInfo = objEntity.GetType().GetProperty("ExceptionText");
        //        propertyInfo.SetValue(objEntity, ex.ToString(), null);
        //        CommonLog.CommonErrorLog(ex, "Da co loi: SetValue cua property" + propertyName);
        //    }
        //}
        /// <summary>
        /// hàm chuyển đổi kiểu dữ liệu
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ChangeType(this object value, Type type)
        {
            try
            {
                if (value == null && type.IsGenericType) return Activator.CreateInstance(type);
                if (value == null) return null;
                if (type == value.GetType()) return value;
                if (type.IsEnum)
                {
                    if (value is string)
                    {
                        return Enum.Parse(type, value as string);
                    }
                    else
                    {
                        return Enum.ToObject(type, value);
                    }
                }
                if (!type.IsInterface && type.IsGenericType)
                {
                    Type innerType = type.GetGenericArguments()[0];
                    object innerValue = ChangeType(value, innerType);
                    return Activator.CreateInstance(type, new object[] { innerValue });
                }
                if (value is string && type == typeof(Guid)) return new Guid(value as string);
                if (value is Guid && type == typeof(string)) return value.ToString();
                if (!(value is IConvertible)) return value;
                return Convert.ChangeType(value, type);
            }
            catch (FormatException)
            {
                return type.GetDefaultValue();
            }
        }
        public static Type GetPrimaryKeyType(this Type type)
        {
            PropertyInfo[] props = type.GetProperties();
            PropertyInfo property = null;
            if (props != null)
            {
                property = props.SingleOrDefault(p => p.GetCustomAttribute(typeof(KeyAttribute), true) != null);
            }
            if (property != null)
            {
                return property.PropertyType; ;
            }
            return typeof(object);
        }
        public static string GetTableNameOnly(this Type type)
        {
            string tableName = ((ConfigTableAttribute)type.GetCustomAttributes(typeof(ConfigTableAttribute), false).FirstOrDefault()).TableName;
            return tableName;
        }
        public static string GetPrimaryKeyFieldName(this Type type)
        {
            return type.GetFieldName(typeof(KeyAttribute));
        }
        public static string GetFieldName(this Type type, Type attrType)
        {
            string primaryKeyName = string.Empty;
            PropertyInfo[] props = type.GetProperties();
            if (props != null)
            {
                var propertyInfoKey = props.SingleOrDefault(p => p.GetCustomAttribute(attrType, true) != null);
                if (propertyInfoKey != null)
                {
                    primaryKeyName = propertyInfoKey.Name;
                }
            }
            return primaryKeyName;
        }
        public static string GetViewOrTableName(this Type type)
        {
            var configTable = ((ConfigTableAttribute)type.GetCustomAttributes(typeof(ConfigTableAttribute), false).FirstOrDefault());
            string tableName = configTable?.ViewName;
            if (string.IsNullOrEmpty(tableName))
            {
                tableName = configTable?.TableName;
            }
            return tableName;
        }
        /// <summary>
        /// lấy về giá trị mặc đinh cho kiểu dữ liệu
        /// </summary>
        /// <param name="type">type</param>
        /// <returns></returns>
        public static object GetDefaultValue(this Type type)
        {
            Expression<Func<object>> e = Expression.Lambda<Func<object>>(Expression.Convert(Expression.Default(type), typeof(object)));
            return e.Compile().DynamicInvoke();
        }
        // ------------------------- thêm bổ sung--------------------
        public static string GetTablename<T>()
        {
            string tableName = string.Empty;
            var tableNameAttribute = typeof(T).GetCustomAttribute(typeof(TableNameAttribute), true) as TableNameAttribute;
            if (tableNameAttribute != null)
            {
                tableName = tableNameAttribute.Name;
            }
            else
            {
                tableName = typeof(T).Name;
            }
            return tableName;
        }
        /// <summary>
        /// chuyen ds thanh object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        public static List<T> ToListObject<T>(this IDataReader dataReader)
        {
            if (dataReader == null) return null;
            List<T> lstResult = new List<T>();
            Type rs = typeof(T);
            if (!rs.IsValueType && !rs.Name.Equals("string"))
            {
                T objectItem = default(T);
                string propertyName = string.Empty;
                while (dataReader.Read())
                {
                    objectItem = Activator.CreateInstance<T>();
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        propertyName = dataReader.GetName(i);
                        object vVal = dataReader[propertyName];
                        objectItem.SetValue(propertyName, vVal);
                    }
                    lstResult.Add(objectItem);
                }
            }
            else
            {
                while (dataReader.Read())
                {
                    lstResult.Add((T)dataReader.GetValue(0));
                }
            }
            return lstResult;
        }

        // chuyển datareader => dictionary
        public static List<Dictionary<string, object>> ToListDictionary(this IDataReader dataReader)
        {
            if (dataReader == null) return null;
            List<Dictionary<string, object>> lstResult = new List<Dictionary<string, object>>();
            try
            {
                while (dataReader.Read())
                {
                    Dictionary<string, object> dicData = new Dictionary<string, object>();
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        string name = dataReader.GetName(i);
                        if (!dicData.ContainsKey(name))
                        {
                            //dicData.Add(name, dataReader.GetValueFromDataReader(i));
                            dicData.Add(name, dataReader.GetValue(i));

                        }
                    }
                    lstResult.Add(dicData);
                }
            }
            catch (Exception ex)
            {
                CommonLog.CommonErrorLog(ex, $"ExtensionMethod.TolistDictionary");
            }
            return lstResult;
        }
        /// <summary>
        /// chuyển string có khả năng injection
        /// </summary>
        /// <param name="tableViewColumnName"></param>
        /// <returns></returns>
        public static string SafeSqlLiteralForObjectName(string tableViewColumnName)
        {
            if (string.IsNullOrEmpty(tableViewColumnName))
            {
                return tableViewColumnName;
            }
            return tableViewColumnName.Replace("`", "``");
        }
    }
}