using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Text;
namespace TMSBO
{
    public class BaseModel : ICloneable
    {
        public object Clone()
        {
            return MemberwiseClone();
        }
        #region properties
        public DateTime CreatedDate { get; set; } = DateTime.Today;

        public DateTime ModifiedDate { get; set; } = DateTime.Today;

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public EntityState State { get; set; } = EntityState.None;
        public string ExceptionText { get; set; }
        public List<ModelDetailConfig> ModelDetailConfigs { get; set; }
        // trả về type của model đang kế thừa
        private Type _type;
        public Type GetEntityType()
        {
            if (_type == null)
            {
                _type = this.GetType();
            }
            return _type;
        }
        /// <summary>
        ///  kiểm tra để biết kiểu dự liệu là autoincrement = false hay guid = true
        /// </summary>
        /// <returns>autoincrement = false hay guid = true</returns>
        public bool HasIdenty()
        {
            return !KeyIsGuid();
        }
        public Type GetPrimaryKeyType()
        {
            return this.GetType().GetPrimaryKeyType();
        }
        /// <summary>
        /// kiểm tra là guid
        /// </summary>
        /// <returns> true = guid ; false = Int</returns>
        public bool KeyIsGuid()
        {
            // lay key, neu key khong null thi la Guid
            var prop = this.GetProperies().FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>(true) != null);
            if (prop == null)
            {
                return false;
            }
            else { return prop.PropertyType == typeof(Guid); }
        }
        /// <summary>
        /// kiểm tra thuộc tính có tồn tại trong model không, nếu không có thì trả về false
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public bool ExistProperty(string propertyName)
        {
            if (this.GetEntityType().GetProperty(propertyName) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
        // ------------------------------- Function get informations --------------------------------
        #region Func get Props
        //private string _queryInsert;
        //private string _queryUpdate;
        private string _primarykeyFieldName;
        private PropertyInfo[] _properties;
        private Tuple<List<string>, List<string>> _tupleColumns = null;
        /// <summary>
        /// get thông tin tất cả thuộc tính
        /// </summary>
        /// <returns>mảng thông tin thuộc tính</returns>
        public PropertyInfo[] GetProperies()
        {
            if (this._properties == null)
            {
                this._properties = this.GetEntityType().GetProperties();
            }
            return this._properties;
        }
        // lấy tên bảng theo attribute
        private string _tableName;
        public virtual string GetTableName()
        {
            if (string.IsNullOrEmpty(this._tableName))
            {
                var tableNameAttribute = this.GetType().GetCustomAttribute(typeof(TableNameAttribute), true) as TableNameAttribute;
                if (tableNameAttribute != null)
                {
                    this._tableName = tableNameAttribute.Name;
                }
                else
                {
                    this._tableName = this.GetEntityType().Name;
                }
            }
            return this._tableName;
        }
        // get primarykey
        public object GetPrimaryKeyValue()
        {
            string pKeyValue = this.GetPrimaryKeyFieldName();
            return this[pKeyValue];
        }
        // get primary by anotation
        public string GetPrimaryKeyFieldName()
        {
            if (string.IsNullOrEmpty(this._primarykeyFieldName))
            {
                PropertyInfo[] props = this.GetProperies();
                if (props.Any())
                {
                    var propertyInforKey = props.SingleOrDefault(p => p.GetCustomAttribute<KeyAttribute>(true) != null);
                    if (propertyInforKey != null)
                    {
                        this._primarykeyFieldName = propertyInforKey.Name;
                    }
                }
            }
            return this._primarykeyFieldName;
        }
        /// <summary>
        /// set value theo tên prop
        /// </summary>
        public void SetValueByAttribute(Type attrType, object value)
        {
            PropertyInfo[] props = this.GetType().GetProperties();
            PropertyInfo property = null;
            if (props != null)
            {
                property = props.SingleOrDefault(p => p.GetCustomAttribute(attrType, true) != null);
            }
            if (property != null)
            {
                property.SetValue(this, value);
            }
        }

        // get or set value for property of BO (indexer) 
        public object this[string propertyName]
        {
            get
            {
                PropertyInfo pi = this.GetEntityType().GetProperty(propertyName);
                if (pi != null)
                {
                    return pi.GetValue(this, null);
                }
                else
                {
                    // ghi log
                    throw new Exception(string.Format("<{0}> does not exist in object <{1}>", propertyName, this.GetEntityType().ToString()));
                }
            }
            set
            {
                PropertyInfo pi = this.GetEntityType().GetProperty(propertyName);
                if (pi != null)
                {
                    pi.SetValue(this, value, null);
                }
                else
                {
                    throw new Exception(string.Format("<{0}> does not exist in object <{1}>", propertyName, this.GetEntityType().ToString()));
                }
            }
        }
        // get list of columns
        // input this.Getproperties()
        public Tuple<List<string>, List<string>> GetColumns()
        {
            if (_tupleColumns == null)
            {
                List<string> columsNormal = new List<string>();
                List<string> columsJson = new List<string>();
                foreach (PropertyInfo p in this.GetProperies())
                {
                    if (p.GetCustomAttribute<ColumnAttribute>(true) != null)
                    {
                        string name = p.Name;
                        if (p.GetCustomAttribute<ColumnJsonAttribute>(true) != null)
                        {
                            columsJson.Add(name);
                        }
                        else
                        {
                            columsNormal.Add(name);
                        }
                    }
                }
                // merge to tuple
                _tupleColumns = new Tuple<List<string>, List<string>>(columsNormal, columsJson);
            }
            return _tupleColumns;
        }
        public void SetAutoPrimaryKey()
        {
            PropertyInfo[] props = this.GetType().GetProperties();
            PropertyInfo propertyInfoKey = null;
            if (props != null)
            {
                propertyInfoKey = props.SingleOrDefault(p => p.GetCustomAttribute<KeyAttribute>(true) != null);
                if (propertyInfoKey != null)
                {
                    if (propertyInfoKey.PropertyType == typeof(long))
                    {
                        
                    }
                    else if (propertyInfoKey.PropertyType == typeof(Int32))
                    {
                        
                    }
                    else if (propertyInfoKey.PropertyType == typeof(Guid))
                    {
                        propertyInfoKey.SetValue(this, Guid.NewGuid()); 
                    }
                    else
                    {
                        //String thường đã có giá trị nên không cần set
                    }
                }
            }
        }
        #endregion

    }
}