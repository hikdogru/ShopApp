using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // localhost:5000/Product/List
        public IActionResult List()
        {
            return View();
        }

        // localhost:5000/Product/Details
        public IActionResult Details(int id)
        {
            return View();
        }
    }
}
