using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class DatabaseContextFactory: IDatabaseContextFactory
    {
        private IDatabaseContext dataContext;

        /// <summary>
        /// If data context remain null then initialize new context 
        /// khởi tạo connectionString
        /// </summary>
        /// <returns>dataContext if dataContext đã khởi tạo ở Database context rồi thì trả về giá trị đã khởi tạo, nếu null thì khởi tại DatabaseContext mới</returns>
        public IDatabaseContext Context()
        {
            return dataContext ?? (dataContext = new DatabaseContext());
        }

        /// <summary>
        /// Dispose existing context
        /// sử dụng hàm thu dọn bộ nhớ để kết hợp giải phóng tài nguyên 
        /// </summary>
        public void Dispose()
        {
            if (dataContext != null)
                dataContext.Dispose();
        }
    }
}
