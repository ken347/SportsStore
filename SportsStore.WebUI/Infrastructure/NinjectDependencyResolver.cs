﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Moq;
using Ninject;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        { 
            kernel = kernelParam;
            AddBinding();
        }

        public object GetService(Type serviceType) => kernel.TryGet(serviceType);

        public IEnumerable<object> GetServices(Type serviceType) => kernel.GetAll(serviceType);

        private void AddBinding()
        {
            // TODO 之後替換實現
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>()
            { 
                new Product()
                { 
                    Name="Football",
                    Price=25
                },
                new Product()
                { 
                    Name="Surf board",
                    Price=179
                },
                new Product()
                { 
                    Name="Running shoes",
                    Price=95
                }
            });

            //ToConstant() 單例
            kernel.Bind<IProductRepository>().ToConstant(mock.Object);
        }
    }
}