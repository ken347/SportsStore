using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            //準備
            Product product = new Product()
            {
                ProductID = 2,
                Name = "Test",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };

            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            { 
                new Product()
                { 
                    ProductID=1,
                    Name="P1"
                },
                product,
                new Product()
                { 
                    ProductID=3,
                    Name="P3"
                }
            }.AsQueryable());

            ProductController target = new ProductController(mock.Object);

            //動作
            ActionResult result = target.GetImage(2);

            //斷言
            //這是一個不太全面的測試，因為FileResult無法訪問二進制的文件內容
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(product.ImageMimeType,((FileResult)result).ContentType);
        }

        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            //準備
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product()
                {
                    ProductID=1,
                    Name="P1"
                },
                new Product()
                {
                    ProductID=2,
                    Name="P2"
                }
            }.AsQueryable());

            ProductController target = new ProductController(mock.Object);

            //動作
            ActionResult result = target.GetImage(100);

            //斷言
            Assert.IsNull(result);
        }
    }
}
