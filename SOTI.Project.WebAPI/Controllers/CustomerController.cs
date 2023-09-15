using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Xebia.Project.DataAccessLayer;

namespace SOTI.Project.WebAPI.Controllers
{
    public class CustomerController : ApiController
    {
        private readonly ICustomer _customer = null;
        public CustomerController(ICustomer customer)
        {
            _customer = customer;
        }

        [HttpGet]
        public IHttpActionResult GetCustomers()
        {
            var dt = _customer.GetDistinctProduct();
            if (dt == null)
                return BadRequest();
            return Ok(dt);
        }
    }
}