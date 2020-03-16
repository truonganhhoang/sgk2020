using System;
using System.Threading.Tasks;
using System.Web.Http;
using TMSBL;
using TMSBO;

namespace TMSAPI.Controllers
{
    [RoutePrefix("api/test")]
    public class TestController : BaseController
    {
        public TestController(ITestBL testBL)
        {
            this.BL = testBL;
            //this.CurrentModelType = typeof(Model.Candidate);
        }
        [HttpGet]
        [Route("value")]
        public string Test()
        {
            return "Hello grand master, API is running!";
        }
        [HttpGet]
        [Route("all")]
        public async Task<ServiceResult> GetDataTest()
        {
            ServiceResult res = new ServiceResult();
            try
            {
                 res.Data = (this.BL as ITestBL).Test();
                //res.Data = this.BL... => gọi thẳng vào base
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return res;
        }
    }
}
