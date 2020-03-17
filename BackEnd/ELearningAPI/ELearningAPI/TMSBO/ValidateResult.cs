using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMSBO
{
   public class ValidateResult
    {
        public object ID { get; set; }
        public string Code { get; set; }
        public string ErrorMessage { get; set; }
        public object AdditionInfo { get; set; }
        public int MyProperty { get; set; }
        public Enumeration.ValidateType ValidateType { get; set; }
    }
}
