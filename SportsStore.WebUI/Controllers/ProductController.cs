using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository _repository;
        public int pageSize = 4;

        public ProductController(IProductRepository repository) => _repository = repository;

        public ViewResult List(string category, int page = 1)
        {
            ProductsListViewModel model = new ProductsListViewModel()
            {
                Products = _repository.Products.OrderBy(p => p.ProductID)
                .Where(p => category == null || p.Catagory == category)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category ==null?
                    _repository.Products.Count():
                    _repository.Products.Where(e=>e.Catagory==category).Count()
                },
                CurrentCategory=category
            };

            return View(model);
        }

        public FileContentResult GetImage(int productId)
        {
            Product product = _repository.Products.FirstOrDefault(p=>p.ProductID==productId);

            if (product != null)
            {
                return File(product.ImageData,product.ImageMimeType);
            }

            return null;
        }
    }
}