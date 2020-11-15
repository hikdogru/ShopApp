using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        // localhost:5000/Home/Index
        public IActionResult Index()
        {
            int our = DateTime.Now.Hour;

            string message = our > 12 ? "Have a nice day" : "Good morning";
            ViewBag.Greeting = message;
            ViewBag.UserName = "John";
            return View();
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
