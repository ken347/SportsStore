using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Moq;
using Ninject;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Concrete;
using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
        { 
            _kernel = kernel;
            AddBinding();
        }

        public object GetService(Type serviceType) => _kernel.TryGet(serviceType);

        public IEnumerable<object> GetServices(Type serviceType) => _kernel.GetAll(serviceType);

        private void AddBinding()
        {
            #region 模擬數據
            //Mock<IProductRepository> mock = new Mock<IProductRepository>();
            //mock.Setup(m => m.Products).Returns(new List<Product>()
            //{
            //    new Product()
            //    {
            //        Name="Football",
            //        Price=25
            //    },
            //    new Product()
            //    {
            //        Name="Surf board",
            //        Price=179
            //    },
            //    new Product()
            //    {
            //        Name="Running shoes",
            //        Price=95
            //    }
            //});

            ////ToConstant() 單例
            //_kernel.Bind<IProductRepository>().ToConstant(mock.Object); 
            #endregion

            _kernel.Bind<IProductRepository>().To<EFProductRepository>();

            EmailSettings emailSettings = new EmailSettings()
            {
                WriteAsFile = bool.Parse(ConfigurationManager.
                AppSettings["Email.WriteAsFile"] ?? "false")
            };

            _kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>()
                .WithConstructorArgument("emailSettings", emailSettings);
        }
    }
}