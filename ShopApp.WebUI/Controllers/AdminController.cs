using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopApp.Business.Abstract;
using ShopApp.Entity;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private IProductService _productService;

        public AdminController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult ProductList()
        {
            return View(new ProductListViewModel()
            {
                Products = _productService.GetAll()
            });
        }
        // Get
        public IActionResult CreateProduct()
        {
            return View();
        }
        // Post
        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            _productService.Create(product);
            return RedirectToAction("ProductList");
        }

    }
}
