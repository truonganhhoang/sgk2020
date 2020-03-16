using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMSBL
{
    public class TestBL : BaseBL, ITestBL
    {
        public object Test()
        {
            //var dic = new Dictionary<string, object>();
            return DL.Query("Select * from test",System.Data.CommandType.Text);
        }
    }
}
