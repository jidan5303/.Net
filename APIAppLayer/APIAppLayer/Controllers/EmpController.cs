using BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APIAppLayer.Controllers
{
    public class EmpController : ApiController
    {
        [HttpGet]
        [Route("api/employees")]
        public HttpResponseMessage AllEmployees()
        {
            try
            {
                var data = EmployeeService.Get();
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
