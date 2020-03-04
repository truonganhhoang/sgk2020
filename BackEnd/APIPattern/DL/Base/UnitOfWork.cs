using MySql.Data.MySqlClient;
using System;

namespace DL
{
    public class UnitOfWork: IUnitOfWork, IDisposable
    {
        private IDatabaseContextFactory _factory;
        private IDatabaseContext _context;
        public MySqlTransaction Transaction { get; private set; }

        //public UnitOfWork(IDatabaseContextFactory factory)
        //{
        //    _factory = factory;
        //}
        //sử dụng hàm khởi tạo của class để khởi tạo 1 thể hiện của IdatabaseContextFactory
        public UnitOfWork()
        {
            _factory = new DatabaseContextFactory();
        }
        /// <summary>
        /// sự kiện commit khi được gọi trong DLBase
        /// </summary>
        public void Commit()
        {
            if (Transaction != null)
            {
                try
                {
                    Transaction.Commit();
                }
                catch (Exception)
                {
                    Transaction.Rollback();
                }
                Transaction.Dispose();
                Transaction = null;
            }
            else
            {
                throw new NullReferenceException("Tryed commit not opened transaction");
            }
        }

        /// <summary>
        /// Define a property of context class
        /// lấy connectionString
        /// </summary>
        public IDatabaseContext DataContext
        {
            get { return _context ?? (_context = _factory.Context()); }
        }

        /// <summary>
        /// Begin a database transaction
        /// bắt đầu transaction đảm bảo toàn vẹn dữ liệu
        /// </summary>
        /// <returns>Transaction</returns>
        public MySqlTransaction BeginTransaction()
        {
            if (Transaction != null)
            {
                throw new NullReferenceException("Not finished previous transaction");
            }
            Transaction = _context.Connection.BeginTransaction();
            return Transaction;
        }

        /// <summary>
        /// giải phóng vùng nhớ + đóng kết nối
        /// hàm được gọi mỗi khi kết thúc transaction
        /// </summary>
        public void Dispose()
        {
            if (Transaction != null)
            {
                Transaction.Dispose();
            }
            if (_context != null)
            {
                _context.Dispose();
            }
        }
    }
}
