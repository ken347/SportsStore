using SportsStore.WebUI.Infrastructure.Abstract;
using SportsStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private IAuthProvider _authProvider;

        public AccountController(IAuthProvider authProvider) => _authProvider = authProvider;   

        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model,string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (_authProvider.Authenticate(model.UserName, model.Password))
            {
                return Redirect(returnUrl ?? Url.Action("Index", "Admin"));
            }

            ModelState.AddModelError("","帳號或密碼錯誤");
            return View();
        }
    }
}