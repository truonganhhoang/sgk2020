using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using BO;
using MySql.Data.MySqlClient;

namespace DL
{
    /// <summary>
    /// Base Repository thực hiện thao tác với CSDL
    /// </summary>
    /// <typeparam name="T">Entity ảnh hưởng</typeparam>
    public class BaseRepository<T> : DLBase, IBaseRepository<T> where T : class, new()
    {
        #region DECLARE


        #endregion

        #region Constructor

        #endregion

        #region Base Method - các phương thức Base
        /// <summary>
        /// Thực thi một store với các tham số truyền vào
        /// </summary>
        /// <param name="storeName">Tên store cần thực thi</param>
        /// <param name="param">mảng các tham số truyền vào</param>
        /// <returns></returns>
        /// Created by: NVCUONG (19/04/2018)
        public new int ExecuteNonQuery(string storeName, object[] param)
        {
            return base.ExecuteNonQuery(storeName, param);
        }

        /// <summary>
        /// Thực thi câu lệnh sql lấy một giá trị đơn trả về
        /// </summary>
        /// <param name="storeName">tên store cần thực thi</param>
        /// <param name="param">mảng các tham số cần truyền vào</param>
        /// <returns>Giá trị đơn cần trả về (có thể là int, string,...)</returns>
        /// Created by: NVCUONG (19/04/2018)
        public new object ExecuteScalar(string storeName, object[] param)
        {
            return base.ExecuteScalar(storeName, param);
        }

        /// <summary>
        /// Thực thi câu lệnh MySql lấy về một (hoặc List) đối tượng
        /// </summary>
        /// <param name="storeName">tên store cần thực thi</param>
        /// <param name="param">mảng các tham số cần truyền vào</param>
        /// <returns>Giá trị đơn cần trả về (có thể là int, string,...)</returns>
        /// Created by: NVCUONG (19/04/2018)
        public new MySqlDataReader ExecuteReader(string storeName, object[] paramValue)
        {
            return base.ExecuteReader(storeName, paramValue);
        }

        /// <summary>
        /// Lấy một mảng dữ liệu cho Entity
        /// </summary>
        /// <returns>Mảng dữ liệu</returns>
        /// Created By: NVCUONG (19/04/2018)
        public IEnumerable<T> GetEntities()
        {
            return base.GetEntities<T>(GenerateProcUtility<T>.GetListEntity());
        }

        /// <summary>
        /// Lấy một mảng dữ liệu cho Entity
        /// </summary>
        /// <param name="storeName">Tên store procedure</param>
        /// <returns>Mảng dữ liệu kiểu Entity</returns>
        /// Created by: NVCUONG (13/04/2018)
        public IEnumerable<T> GetEntities(string storeName)
        {
            return base.GetEntities<T>(storeName);
        }

        /// <summary>
        /// Lấy mảng dữ liệu Entity
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="storeName"> Tên store</param>
        /// <param name="paramValue">mảng các tham số truyền vào</param>
        /// <returns>List Entity</returns>
        /// Created By: nvcuong (30/06/2015)
        public IEnumerable<T> GetEntities(string storeName, object[] paramValue)
        {
            return base.GetEntities<T>(storeName, paramValue);
        }

        /// <summary>
        /// Thêm mới Entity - sử dụng store thực thi tự sinh trên chương trình
        /// </summary>
        /// <param name="entity">entity truyền vào</param>
        /// <returns>Số lượng bản ghi thêm mới thành công</returns>
        /// Created by: NVCUONG (17/04/2018)
        public int Insert(T entity)
        {
            string storeInsertName = GenerateProcUtility<T>.InsertEntity();
            return Update(entity, storeInsertName);
        }

        /// <summary>
        /// Thêm mới Entity - tự truyền tên store sẽ thực thi
        /// </summary>
        /// <param name="entity">entity truyền vào</param>
        /// <param name="storeName">Tên store thực hiện thêm mới</param>
        /// <returns>Số lượng bản ghi thêm mới thành công</returns>
        /// Created by: NVCUONG (17/04/2018)
        public int Insert(T entity, string storeInsertName)
        {
            return Update(entity, storeInsertName);
        }

        /// <summary>
        /// Cập nhật (có thể là thêm mới hoặc sửa) Entity (store thực hiện cập nhật chương trình sẽ tự sinh theo Template)
        /// </summary>
        /// <param name="entity">Entity truyền vào</param>
        /// <returns>Số lượng bản ghi cập nhật thành công</returns>
        /// Created By: NVCUONG (17/04/2017)
        public int Update(T entity)
        {
            return base.Update<T>(entity, GenerateProcUtility<T>.UpdateEntity());
        }

        /// <summary>
        /// Thêm, sửa Entity (tùy biến Store truyền vào)
        /// </summary>
        /// <typeparam name="T">entity</typeparam>
        /// <typeparam name="storeName">Tên store thực hiện cập nhật dữ liệu</typeparam>
        /// Người tạo: nvcuong
        /// Ngày tạo: 30/06/2015
        /// <returns>Số lượng bản ghi thêm/sửa thành công</returns>
        public int Update(T entity, string storeName)
        {
            return base.Update<T>(entity, storeName);
        }



        /// <summary>
        /// Xóa entity
        /// </summary>
        /// <param name="entity">Entity cần xóa (hàm chưa hoàn chỉnh)</param>
        /// <returns></returns>
        /// Created by: NVCUONG (17/04/2017)
        public virtual int Delete()
        {
            return base.Delete(GenerateProcUtility<T>.DeleteEntityByPrimaryKey(), new object[] { });
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
        public override int Delete(string storeName, string[] nameParameters, object[] valueParameters, int numberParameter)
        {
            return base.Delete(storeName, nameParameters, valueParameters, numberParameter);
        }

        /// <summary>
        /// Xóa dữ liệu với nhiều tham số đầu vào
        /// </summary>
        /// <param name="sql">Tên store</param>
        /// <param name="param">Mảng các tham số truyền vào</param>
        /// <returns></returns>
        /// Created by: NVCUONG (19/04/2018)
        public override int Delete(string storeName, object[] param)
        {
            return base.Delete(storeName, param);
        }
        #endregion

        #region "GetData"

        ///// <summary>
        ///// Lấy danh sách chứa toàn bộ các đối tượng
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <returns></returns>
        public IEnumerable<T> GetEntitiesAll()
        {
            string storeName = GenerateProcUtility<T>.GetEntities();
            return GetEntities(storeName);
        }

        public virtual T GetEntityById(object id)
        {
            string storeName = GenerateProcUtility<T>.GetEntityById();
            return GetEntities(storeName, (new object[] { id })).FirstOrDefault();
        }

        /// <summary>
        /// Lấy thông tin đối tượng theo ID của đối tượng
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="entityId">Khóa chính của Entity</param>
        /// <returns>Entity</returns>
        /// Created by: NVCUONG (20/04/2018)
        public T GetEntityByEntityId(object entityId)
        {
            string storeName = GenerateProcUtility<T>.GetEntityById();
            return GetEntities(storeName, (new object[] { entityId })).FirstOrDefault();
        }

        /// <summary>
        /// Lấy danh sách đối tượng theo tham số (test)
        /// </summary>
        /// <typeparam name="T">Đối tượng tác động</typeparam>
        /// <param name="param">Mảng các tham số</param>
        /// <returns>Danh sách dữ liệu</returns>
        /// Created by: NVCUONG (20/06/2018)
        public IEnumerable<T> GetEntities_ByMultiParam(object param)
        {
            string storeName = GenerateProcUtility<T>.GetListEntity_ByMultiParam();
            return GetEntities(storeName, (new object[] { param }));
        }

        /// <summary>
        /// Lấy danh sách đối tượng theo tham số (test)
        /// </summary>
        /// <typeparam name="T">Đối tượng tác động</typeparam>
        /// <param name="param">Mảng các tham số</param>
        /// <returns>Danh sách dữ liệu được phân trang</returns>
        /// Created by: NVCUONG (20/06/2018)
        public Paging<T> SelectEntitiesPaging(object[] param)
        {
            string storeName = GenerateProcUtility<T>.SelectEntitiesPaging();
            return SelectEntitiesPaging<T>(storeName, param);
        }

        #endregion

        #region "Update/Insert Data"
        public int UpdateEntity(T entity)
        {
            string storeName = GenerateProcUtility<T>.UpdateEntity();
            return Update(entity, storeName);
        }

        public int InsertEntity(T entity)
        {
            string storeName = GenerateProcUtility<T>.InsertEntity();
            return Update(entity, storeName);
        }

        /// <summary>
        /// Xóa đối tượng theo id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Created by: NVCUONG (05/03/2018)
        public int DeleteEntityById(object id)
        {
            string storeName = GenerateProcUtility<T>.DeleteEntityByPrimaryKey();
            var entity = Activator.CreateInstance<T>();
            string tableName = entity.GetType().Name;
            return Delete(storeName, new object[] { id });
        }

        /// <summary>
        /// Xóa đối tượng theo với nhiều tham số đầu vào
        /// </summary>
        /// <typeparam name="T">entiy</typeparam>
        /// <param name="param">mảng các tham số truyền vào</param>
        /// <returns></returns>
        /// Created by: NVCUONG (05/03/2018)
        public int DeleteEntity_ByMultiParam(object[] param)
        {
            string strProc = GenerateProcUtility<T>.DeleteEntityByPrimaryKey();
            var entity = Activator.CreateInstance<T>();
            string tableName = entity.GetType().Name;
            return Delete(strProc, new object[] { param });
        }
        #endregion

        //************************************************************************************

        #region MyRegion

        #endregion



        #region OTHER
        protected virtual void InsertCommandParameters(T entity, MySqlCommand cmd) { }
        protected virtual void UpdateCommandParameters(T entity, MySqlCommand cmd) { }
        protected virtual void DeleteCommandParameters(T entity, MySqlCommand cmd) { }
        protected virtual T Map(MySqlDataReader reader) { return null; }
        protected virtual List<T> Maps(MySqlDataReader reader) { return null; }

        Paging<T> IBaseRepository<T>.SelectEntitiesPaging(object[] param)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
