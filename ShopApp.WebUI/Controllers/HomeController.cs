using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopApp.Business.Abstract;
using ShopApp.WebUI.ViewModels;
using ShopApp.DataAccess.Abstract;

namespace ShopApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IProductService _productService;
        public HomeController(IProductService productService)
        {
            this._productService = productService;
        }
        // localhost:5000/Home/Index
        public IActionResult Index()
        {

            var productViewModel = new ProductListViewModel() { Products = _productService.GetHomePageProducts()};
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
