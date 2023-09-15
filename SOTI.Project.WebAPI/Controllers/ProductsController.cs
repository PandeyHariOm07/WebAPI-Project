using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Xebia.Project.DataAccessLayer;

namespace SOTI.Project.WebAPI.Controllers
{
    public class ProductsController : ApiController
    {
        //public IHttpActionResult GetData()
        //{
        //    return Json(new { Message= "Welcome" });
        //}
        private readonly IProduct _product = null;
        public ProductsController()
        {
            _product = new ProductDetailsConnect();
        }


        [HttpGet]
        public IHttpActionResult GetProducts()
        {
           var dt = _product.GetProduct();
            if (dt == null)
                return BadRequest();
            return Ok(dt);
        }

        [HttpGet]
        public IHttpActionResult GetProductById(int id)
        {
            var row = _product.GetProductById(id);
            if (row == null)
                return BadRequest();
            return Ok(row);
        }

        [HttpPost]
        public IHttpActionResult AddProduct(Product product)
        {
            var res = _product.AddProduct(product.ProductName,product.unitPrice.Value,product.UnitsInStock.Value);
            if (res) return Ok();
            return BadRequest();
        }

        [HttpPut]
        public IHttpActionResult UpdateProduct([FromUri] int id, [FromBody] Product product)
        {
            var res = _product.UpdateProduct(id, product);
            if (res)
            {
                return Ok();
            }
            return BadRequest();

        }
        [HttpDelete]
        public IHttpActionResult DeleteProduct([FromUri] int id)
        {
            var res = _product.DeleteProduct(id);
            if (res) return Ok();
            return BadRequest();
        }
    }
}
