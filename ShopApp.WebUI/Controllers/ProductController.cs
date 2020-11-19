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
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            // ViewBag
            // Model
            // ViewData

            var product = new Product
            { ProductId  = 2, Name = "Samsung Galaxy S20 Ultra", Price = 1000, Description = "Very nice telephone." };
            ViewData["Product"] = product;
            ViewData["Category"] = "Smart Phones";

            ViewBag.Category = "Smart Phones";
            ViewBag.Product = product;
            return View(product);
        }

        // localhost:5000/Product/List
        public IActionResult List(int? id)
        {
            var products = ProductRepository.Products;
            if (id != null)
            {
                products = products.Where(p => p.CategoryId == id).ToList();
            }
            
            var productViewModel = new ProductViewModel(){Products = products};
            return View(productViewModel);
        }

        // localhost:5000/Product/Details
        public IActionResult Details(int id)
        {
            var product = ProductRepository.GetProductById(id);
            return View(product);
        }
    }
}
