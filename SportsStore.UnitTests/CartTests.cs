using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            //準備
            Product p1 = new Product()
            {
                ProductID=1,
                Name="P1"
            };

            Product p2 = new Product()
            {
                ProductID = 2,
                Name = "P2"
            };

            Cart target = new Cart();

            //動作
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            CartLine[] results = target.Lines.ToArray();

            //斷言
            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Product, p1);
            Assert.AreEqual(results[1].Product,p2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            //準備
            Product p1 = new Product()
            {
                ProductID = 1,
                Name = "P1"
            };

            Product p2 = new Product()
            {
                ProductID = 2,
                Name = "P2"
            };

            Cart target = new Cart();

            //動作
            target.AddItem(p1,1);
            target.AddItem(p2,1);
            target.AddItem(p1,10);
            CartLine[] results = target.Lines.OrderBy(c=>c.Product.ProductID).ToArray();

            //斷言
            Assert.AreEqual(results.Length,2);
            Assert.AreEqual(results[0].Quantity, 11);
            Assert.AreEqual(results[1].Quantity,1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            //準備
            Product p1 = new Product()
            {
                ProductID = 1,
                Name = "P1"
            };
            Product p2 = new Product()
            {
                ProductID = 2,
                Name = "P2"
            };
            Product p3 = new Product()
            {
                ProductID = 3,
                Name = "P3"
            };

            Cart target= new Cart();

            target.AddItem(p1,1);
            target.AddItem(p2, 3);
            target.AddItem(p3,5);
            target.AddItem(p2, 1);

            //動作
            target.RemoveLine(p2);

            //斷言
            Assert.AreEqual(target.Lines.Where(c=>c.Product==p2).Count(),0);
            Assert.AreEqual(target.Lines.Count(),2);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            //準備
            Product p1 = new Product()
            {
                ProductID = 1,
                Name = "P1",
                Price=100M
            };
            Product p2 = new Product()
            {
                ProductID = 2,
                Name = "P2",
                Price=50M
            };

            Cart target = new Cart();

            //動作
            target.AddItem(p1,1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);
            decimal result = target.ComputeTotalValue();

            //斷言
            Assert.AreEqual(result,450M);

        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            //準備
            Product p1 = new Product()
            {
                ProductID = 1,
                Name = "P1",
                Price = 100M
            };
            Product p2 = new Product()
            {
                ProductID = 2,
                Name = "P2",
                Price = 50M
            };

            Cart target = new Cart();

            target.AddItem(p1,1);
            target.AddItem(p2,1);

            //動作
            target.Clear();

            //斷言
            Assert.AreEqual(target.Lines.Count(), 0);
        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            //準備
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
                { 
                    new Product()
                    { 
                        ProductID=1,
                        Name="P1",
                        Catagory="Apples"
                    }
                }.AsQueryable());

            Cart cart = new Cart();
            CartController target = new CartController(mock.Object);

            //動作
            target.Add2Cart(cart,1,null);

            //斷言
            Assert.AreEqual(cart.Lines.Count(),1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductID, 1);
        }

        //測試將一個產品添加進購物車後，應將用戶重定向到Index視圖
        [TestMethod]
        public void Adding_Product_To_Cart_Goes_To_Cart_Screen()
        {
            //準備
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
                {
                    new Product()
                    {
                        ProductID=1,
                        Name="P1",
                        Catagory="Apples"
                    }
                }.AsQueryable());

            Cart cart = new Cart();
            CartController target = new CartController(mock.Object);

            //動作
            RedirectToRouteResult result = target.Add2Cart(cart, 2, "myUrl");

            //斷言
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        //測試Index動作方法
        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            //準備
            Cart cart = new Cart();
            CartController target = new CartController(null);

            //動作
            CartIndexViewModel result = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;
            //也可以直接用Model屬性，Model屬性底層也是調用ViewData.Model
            //CartIndexViewModel result = (CartIndexViewModel)target.Index(cart, "myUrl").Model;

            //斷言
            Assert.AreSame(result.Cart,cart);
            Assert.AreEqual(result.ReturnUrl,"myUrl");
        }
    }
}
