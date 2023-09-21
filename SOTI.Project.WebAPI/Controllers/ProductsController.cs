using SOTI.Project.WebAPI.CustomFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Xebia.Project.DataAccessLayer;

namespace SOTI.Project.WebAPI.Controllers
{
    [EnableCors(origins:"*",headers:"*",methods:"*")]
    [RoutePrefix("api/Products")]
    public class ProductsController : ApiController
    {
        //public IHttpActionResult GetData()
        //{
        //    return Json(new { Message= "Welcome" });
        //}

        private readonly IProduct _product = null;
        public ProductsController(IProduct product)
        {
            _product = product;
            //_product = new ProductDetailsConnect();
        }


        [HttpGet]

        public IHttpActionResult GetProducts()
        {
            try
            {
                var dt = _product.GetProduct();
                if (dt == null)
                    return BadRequest();
                return Ok(dt);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("ProductId/{id}")]
        [BasicAuthentication]
        public IHttpActionResult GetProductById(int id)
        {
            var row = _product.GetProductById(id);
            if (row == null)
                return BadRequest();
            return Ok(row);
        }

        [HttpPost]
        public IHttpActionResult AddProduct([FromBody]Product product)
        {
            var res = _product.AddProduct(product.ProductName,product.unitPrice.Value,product.UnitsInStock.Value);
            if (res) return Ok();
            return BadRequest();
        }

        [HttpPut]
        [Route("Update/{id}")]
        public IHttpActionResult UpdateProduct([FromUri] int id, [FromBody] Product product)
        {
            if (id != product.ProductId) return BadRequest();
            var res = _product.UpdateProduct(id, product);
            if (res)
            {
                return Ok();
            }
            return BadRequest();

        }
        [HttpDelete]
        [Route("Delete/{id}")]
        public IHttpActionResult DeleteProduct([FromUri] int id)
        {
            var res = _product.DeleteProduct(id);
            if (res) return Ok();
            return BadRequest();
        }
    }
}
