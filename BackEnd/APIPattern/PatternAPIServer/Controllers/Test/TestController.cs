using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PatternAPIServer.Controllers
{
    public class TestController : ApiController
    {
        /// <summary>
        /// inject đối tượng service
        /// created by:nvcuong(2/10/2018)
        /// </summary>
        private readonly IBLTest _bl;
        public TestController(IBLTest baseBL)
        {
            _bl = baseBL;
        }

        [HttpGet]
        [Route("Test")]
        public async Task<HttpResponseMessage> Test()
        {
            HttpResponseMessage res = new HttpResponseMessage();
            try
            {
                var result = _bl.GetData();
                res = Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                res = Request.CreateResponse(HttpStatusCode.BadGateway, ex.Message);
            }
            return await Task.FromResult(res);
        }
    }
}
