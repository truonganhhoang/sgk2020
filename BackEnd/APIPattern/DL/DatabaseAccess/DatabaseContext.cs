using Library;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;


namespace DL
{
    /// <summary>
    /// lớp khởi tạo kết nối đến với Database thông qua connectionString
    /// </summary>
    public class DatabaseContext: IDatabaseContext
    {
        private readonly string _connectionString;
        private MySqlConnection _connection;
        //khởi tạo connection string thông qua hàm khởi tạo của đối tượng
        public DatabaseContext()
        {
            string keyConnection = CommonUtility.GetAppSettingByKey("KeyConnection");
            _connectionString = ConfigurationManager.ConnectionStrings[keyConnection].ConnectionString;
        }
        /// <summary>
        /// tạo kết nối mới triển khai từ interface IDatabaseContext kết quả trả về là 1 trạng thái mở kết nối
        /// </summary>
        public MySqlConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new MySqlConnection(_connectionString);
                }
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                return _connection;
            }
        }

        /// <summary>
        /// sử dụng hàm thu dọn bộ nhớ để đóng kết nối
        /// </summary>
        public void Dispose()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
                _connection.Close();
        }
    }
}
