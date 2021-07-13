using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SportsStore.WebUI.Models;
using SportsStore.WebUI.HtmlHelpers;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        public object ProductListViewModel { get; private set; }

        [TestMethod]
        public void Can_Paginate()
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
                    },
                    new Product()
                    {
                        ProductID=3,
                        Name="P3"
                    },
                    new Product()
                    {
                        ProductID=4,
                        Name="P4"
                    },
                    new Product()
                    {
                        ProductID=5,
                        Name="P5"
                    },
                });

            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            //動作
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null,2).Model;

            //斷言
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length==2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            //準備
            HtmlHelper myHelper = null;
            PagingInfo pagingInfo = new PagingInfo()
            {
                CurrentPage=2,
                TotalItems=28,
                ItemsPerPage=10
            };
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            //動作
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            //斷言
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
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
                    },
                    new Product()
                    {
                        ProductID=3,
                        Name="P3"
                    },
                    new Product()
                    {
                        ProductID=4,
                        Name="P4"
                    },
                    new Product()
                    {
                        ProductID=5,
                        Name="P5"
                    },
                });

            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            //動作
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null,2).Model;

            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage,2);
            Assert.AreEqual(pageInfo.ItemsPerPage,3);
            Assert.AreEqual(pageInfo.TotalItems,5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            //準備
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
                {
                    new Product()
                    {
                        ProductID=1,
                        Name="P1",
                        Catagory="Cat1"
                    },
                    new Product()
                    {
                        ProductID=2,
                        Name="P2",
                        Catagory="Cat2"
                    },
                    new Product()
                    {
                        ProductID=3,
                        Name="P3",
                        Catagory="Cat1"
                    },
                    new Product()
                    {
                        ProductID=4,
                        Name="P4",
                        Catagory="Cat2"
                    },
                    new Product()
                    {
                        ProductID=5,
                        Name="P5",
                        Catagory="Cat3"
                    },
                });

            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            //動作
            Product[] result = ((ProductsListViewModel)controller.List("Cat2", 1).Model)
                .Products.ToArray();


            //斷言
            Assert.AreEqual(result.Length,2);
            Assert.IsTrue(result[0].Name == "P2" && result[0].Catagory == "Cat2");
            Assert.IsTrue(result[1].Name == "P4"&& result[0].Catagory == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
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
                    },
                    new Product()
                    {
                        ProductID=2,
                        Name="P2",
                        Catagory="Apples"
                    },
                    new Product()
                    {
                        ProductID=3,
                        Name="P3",
                        Catagory="Plums"
                    },
                    new Product()
                    {
                        ProductID=4,
                        Name="P4",
                        Catagory="Oranges"
                    }
                });

            NavController target= new NavController(mock.Object);

            //動作
            string[] results = ((IEnumerable<string>)target.Menu().Model).ToArray();

            //斷言
            Assert.AreEqual(results.Length,3);
            Assert.AreEqual(results[0],"Apples");
            Assert.AreEqual(results[1], "Oranges");
            Assert.AreEqual(results[2],"Plums");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            //準備
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m=>m.Products).Returns(new Product[]
                {
                     new Product()
                    {
                        ProductID=1,
                        Name="P1",
                        Catagory="Apples"
                    },
                    new Product()
                    {
                        ProductID=4,
                        Name="P2",
                        Catagory="Oranges"
                    }
                });

            NavController target = new NavController(mock.Object);
            string categoryToSelect = "Apples";

            //動作
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            //斷言
            Assert.AreEqual(categoryToSelect,result);

        }
    }
}
