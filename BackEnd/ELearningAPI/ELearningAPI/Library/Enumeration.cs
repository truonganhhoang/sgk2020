using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Enumeration
    {
        /// <summary>
        /// Enum giới tính
        /// </summary>
        /// Created by: NVCUONG (12/03/2017)
        public enum Gender
        {
            Male = 0,
            Female = 1,
            Other = 2
        }

        /// <summary>
        /// Enum tình trạng hôn nhân
        /// </summary>
        public enum ValidateType : int
        {
            Loi_database=97,
            Loi_server=500,
            Loi_store=98
        }
        public enum HttpStatusCode
        {
            OK=200,
            ServerError=500,
        }
    }
}
