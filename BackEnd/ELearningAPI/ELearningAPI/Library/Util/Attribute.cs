using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class CodeAttribute : Attribute
    {
    }
    public class NameAttribute : Attribute
    {
    }
    public class ParrentAttribute : Attribute
    {
    }

    /// <summary>
    /// lấy custom attribute
    /// </summary>
    public class ColumnAttribute : Attribute
    {
    }
    public class ColumnJsonAttribute : Attribute
    {
    }
    public class TableNameAttribute : Attribute
    {
        public string Name { get; set; }
        public TableNameAttribute()
        {
            this.Name = string.Empty;
        }
        public TableNameAttribute(string name)
        {
            this.Name = name;
        }
    }
    public class ViewAttribute : Attribute
    {
        public string ViewName { get; set; }
        public ViewAttribute(string viewName)
        {
            this.ViewName = viewName;
        }
    }
    /// <summary>
    /// Lưu các thuộc tính cấu hình của bảng
    /// </summary>
    public class ConfigTableAttribute : System.Attribute
    {
        /// <summary>
        /// Tên bảng
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// Tên view
        /// </summary>
        public string ViewName { get; set; }
        /// <summary>
        /// Có cột EditVersion không
        /// </summary>
        public bool HasEditVersion { get; set; }

        /// <summary>
        /// Danh sách cột Unique cách nhau bởi ;
        /// </summary>
        public string FieldUnique { get; set; }
        /// <summary>
        /// Khởi tạo
        /// </summary>
        /// <param name="tableName">Tên bảng</param>
        /// <param name="viewName">Tên view</param>
        /// <param name="hasEditVersion">Có cột EditVersion không</param>
        /// <param name="fieldUnique">Danh sách các trường unique phân cách bởi dấu ;</param>
        public ConfigTableAttribute(string tableName, string viewName = "", bool hasEditVersion = false, string fieldUnique = "")
        {
            TableName = tableName;
            ViewName = viewName;
            HasEditVersion = hasEditVersion;
            FieldUnique = fieldUnique;
        }

    }
}
