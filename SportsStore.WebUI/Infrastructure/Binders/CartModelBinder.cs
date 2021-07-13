using SportsStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Infrastructure.Binders
{
    public class CartModelBinder : IModelBinder
    {
        private const string sessionKey = "Cart";

        //ControllerContext能夠訪問控制器類所具有的全部信息，包含客戶端請求的細節
        //ModelBindingContext能夠提供要求你建立的模型對象以及使綁定過程更易於處理
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Cart cart = null;
            if (controllerContext.HttpContext.Session != null)
                cart = (Cart)controllerContext.HttpContext.Session[sessionKey];


            if (cart == null)
            {
                cart = new Cart();
                if(controllerContext.HttpContext.Session!=null)
                    controllerContext.HttpContext.Session[sessionKey] = cart;
            }

            return cart;
        }
    }
}