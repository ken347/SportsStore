using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository _repository;

        public CartController(IProductRepository repository) => _repository = repository;

        [HttpPost]
        public RedirectToRouteResult Add2Cart(int productId,string returnUrl)
        {
            Product product = _repository.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                GetCart().AddItem(product,1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(int productId,string returnUrl)
        {
            Product product = _repository.Products.FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                GetCart().RemoveLine(product);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        private Cart GetCart()
        {
            // TODO 替換實現，改成Redis

            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
    }
}