﻿using SportsStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IProductRepository _repository;

        public NavController(IProductRepository repository) => _repository = repository;

        public PartialViewResult Menu()
        {
            IEnumerable<string> categories = _repository.Products
                .Select(x => x.Catagory)
                .Distinct()
                .OrderBy(x => x);
            return PartialView(categories);
        }
    }
}