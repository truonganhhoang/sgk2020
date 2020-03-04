using BO;
using DL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Web.Http;

namespace PatternAPIServer.Controllers
{
    public class BaseController<T> : ApiController where T : BaseModel
    { }
}
