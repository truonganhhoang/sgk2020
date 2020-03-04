using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;

namespace BL
{
    public class BLTest : BLEntity<Test>, IBLTest
    {
         IDLTest _dlTest;
        public BLTest(IDLTest repository) : base(repository)
        {
            _dlTest =repository;
        }
    }
}
