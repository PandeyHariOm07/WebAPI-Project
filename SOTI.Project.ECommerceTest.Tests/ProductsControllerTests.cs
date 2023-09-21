using AutoFixture;
using Moq;
using NUnit;
using NUnit.Framework;
using SOTI.Project.WebAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Results;
using Xebia.Project.DataAccessLayer;

namespace SOTI.Project.ECommerceTest.Tests
{
    [TestFixture]
    public class ProductsControllerTests
    {
        private Mock<IProduct> _productMock;
        private ProductsController _controller;
        private readonly Fixture _fixture;

        public ProductsControllerTests()
        {
            _productMock = new Mock<IProduct>();
            _fixture = new Fixture();
        }

        [SetUp]
        public void SetUp()
        {
            _controller = new ProductsController(_productMock.Object)
            {
                Configuration = new HttpConfiguration(),
                Request = new System.Net.Http.HttpRequestMessage()
            };
        }

        [Test]

        //Arrange
        public void GetProducts_shouldReturnListOfProduct_WhenProductExist()
        {
            var productList = _fixture.CreateMany<Product>(5).ToList();
            _productMock.Setup(p => p.GetProduct()).Returns(productList); //SetUP Database Action

            //ACT
            var actionResult = _controller.GetProducts();
            var responce = actionResult as OkNegotiatedContentResult<List<Product>>;

            //Assert
            Assert.IsNotNull(actionResult); //checking if it is null
            Assert.AreEqual(200,(int)responce.ExecuteAsync(CancellationToken.None).Result.StatusCode);
        }

        [Test]
        public void GetProducts_ShouldReturnsBadRequest_WhenExceptionThrown()
        {
            //Arrange
            _productMock.Setup(p => p.GetProduct()).Throws<Exception>();

            //Act
            var actionResult = _controller.GetProducts();
            var res = actionResult as BadRequestErrorMessageResult;

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, res.ExecuteAsync(CancellationToken.None).Result.StatusCode);
        }
    }
}
