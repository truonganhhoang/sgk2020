using System;

namespace DL
{
    /// <summary>
    /// Sinh các chuỗi tên Store truy vấn dữ liệu
    /// </summary>
    public static class GenerateProcUtility<T>
    {
        /// <summary>
        /// Tên Table
        /// </summary>
        private static string _tableName;
        private static string _storeName = string.Empty;

        /// <summary>
        /// Khởi tạo
        /// </summary>
        static GenerateProcUtility()
        {
            var entity = Activator.CreateInstance<T>();
            _tableName = entity.GetType().Name;
        }
        public static string GetEntities()
        {
            _storeName = String.Format("dbo.Proc_Get{0}s", _tableName);
            return _storeName;
        }
        public static string GetNumberOfRecord()
        {
            _storeName = String.Format("dbo.Proc_GetNumberOf{0}", _tableName);
            return _storeName;
        }
        /// <summary>
        /// Lấy tên store lấy thông tin Entity theo khóa chính
        /// </summary>
        /// <returns>tên store lấy dữ liệu Entity theo mã</returns>
        /// Created by: NVCUONG (19/04/2018)
        public static string GetEntityById()
        {
            _storeName = String.Format("dbo.Proc_Get{0}ById", _tableName);
            return _storeName;
        }
        public static string GetListEntity()
        {
            _storeName = String.Format("dbo.Proc_GetList{0}", _tableName);
            return _storeName;
        }
       
        /// <summary>
        /// Lấy tên ra List Entity theo nhiều tham số truyền vào
        /// </summary>
        /// <returns></returns>
        /// Created by: NVCUONG (19/04/2018)
        public static string GetListEntity_ByMultiParam()
        {
            _storeName = String.Format("dbo.Proc_GetList{0}_ByMultiParam", _tableName);
            return _storeName;
        }

        /// <summary>
        /// Lấy tên ra List Entity theo nhiều tham số truyền vào
        /// </summary>
        /// <returns></returns>
        /// Created by: NVCUONG (19/04/2018)
        public static string SelectEntitiesPaging()
        {
            _storeName = String.Format("[dbo].[Proc_Select{0}sPaging]", _tableName);
            return _storeName;
        }

        /// <summary>
        /// Lấy tên store thêm mới Entity
        /// </summary>
        /// <returns>tên store</returns>
        /// Created by: NVCUONG (19/04/2018)
        public static string InsertEntity()
        {
            _storeName = String.Format("dbo.Proc_Insert{0}", _tableName);
            return _storeName;
        }

        /// <summary>
        /// Lấy tên store cập nhật thông tin Entity
        /// </summary>
        /// <returns>tên store cập nhật thông tin Entity</returns>
        /// Created by: NVCUONG (19/04/2018)
        public static string UpdateEntity()
        {
            _storeName = String.Format("dbo.Proc_Update{0}", _tableName);
            return _storeName;
        }

        /// <summary>
        /// Lấy tên store xóa Entity theo khóa chính entity
        /// </summary>
        /// <returns>tên store</returns>
        /// Created by: NVCUONG (19/04/2018)
        public static string DeleteEntityByPrimaryKey()
        {
            _storeName = String.Format("dbo.Proc_Delete{0}By{0}ID", _tableName);
            return _storeName;
        }
    }
}
