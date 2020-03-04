using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Result
    {
        public int ResultCode { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public object Param { get; set; }
    }
}
