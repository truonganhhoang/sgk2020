
using BO;
using DL;
using System;
using System.Collections.Generic;

namespace BL
{
    /// <summary>
    /// Abstract class Entity dùng chung
    /// </summary>
    /// <typeparam name="T">Entity</typeparam>

    public abstract class BLEntity<T> : IBLEntity<T> where T : BaseEntity
    {
        //IUnitOfWork _unitOfWork;
        IBaseRepository<T> _repository;
        /// <summary>
        /// Khởi tạo Entity Service
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="repository"></param>

        public BLEntity(IBaseRepository<T> repository)
        {
            //_unitOfWork = unitOfWork;
            _repository = repository;
        }

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="entity">Entity truyền vào</param>
        /// Created by: NVCUONG
        public virtual int Create(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity is null");
            return _repository.Insert(entity);
            //_unitOfWork.Commit();
        }

        /// <summary>
        /// Sửa Entity
        /// </summary>
        /// <param name="entity">Entity truyền vào</param>
        /// Created by: NVCUONG
        public virtual int Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity is null");
            return _repository.Update(entity);
            
        }

        /// <summary>
        /// Xóa Entity
        /// </summary>
        /// <param name="entity">Entity truyền vào</param>
        /// Created by: NVCUONG
        public virtual void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity is null");
            _repository.Delete();
            //_unitOfWork.Commit();
        }

        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <returns></returns>
        /// Created by: NVCUONG
        public virtual IEnumerable<T> GetAll()
        {
            return _repository.GetEntitiesAll();
        }

        public T GetEntityById(object id)
        {
            return _repository.GetEntityById(id);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public Paging<T> SelectEntitiesPaging(object[] param)
        {
            return _repository.SelectEntitiesPaging(param);
        }

        public object GetData()
        {
           return _repository.GetData();
        }
    }
}
