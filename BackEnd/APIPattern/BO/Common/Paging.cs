using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Paging<T>
    {
        public List<T> Entities { get; set; }
        public int TotalPage { get; set; }
        public int TotalRecord { get; set; }
    }
}
