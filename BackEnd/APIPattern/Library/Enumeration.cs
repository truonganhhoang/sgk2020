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
        public enum MaritalStatus:int
        {
            //Độc thân
            Singler=0,
            //Đã đính hôn
            Engaged=1,
            //Đã kết hôn
            Married=2,
            //Đã ly hôn
            Separated=3,
            //Đã ly thân
            Divorced=4,
            //Góa chồng
            Widow=5,
            //Góa vợ
            Widower=6
        }
    }
}
