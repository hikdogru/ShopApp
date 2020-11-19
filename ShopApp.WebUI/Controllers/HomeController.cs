using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopApp.WebUI.Data;
using ShopApp.WebUI.Models;
using ShopApp.WebUI.ViewModels;

namespace ShopApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        // localhost:5000/Home/Index
        public IActionResult Index()
        {

            var productViewModel = new ProductViewModel() { Products = ProductRepository.Products };
            return View(productViewModel);
        }
        // localhost:5000/Home/About
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}
