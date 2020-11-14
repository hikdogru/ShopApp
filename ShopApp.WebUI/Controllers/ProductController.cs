using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            { Id = 2, Name = "Samsung Galaxy S20 Ultra", Price = 1000, Description = "Very nice telephone." };
            ViewData["Product"] = product;
            ViewData["Category"] = "Smart Phones";

            ViewBag.Category = "Smart Phones";
            ViewBag.Product = product;
            return View(product);
        }

        // localhost:5000/Product/List
        public IActionResult List()
        {

            var products = new List<Product>()
            {
                new Product() {Id = 1, Name = "Galaxy S10", Price = 600, Description = "Good phone", IsApproved = false},
                new Product() {Id = 2, Name = "Galaxy S20", Price = 800, Description = "Very nice phone", IsApproved = true},
                new Product() {Id = 2, Name = "Xioami Mi 10", Price = 800, Description = "Incredible phone", IsApproved = true}
            };

            var category = new Category(){Id = 1, Name = "Smart Phones", Description = "This is category of smart phones."
            }; 
            //ViewBag.Category = category;
            var productViewModel = new ProductViewModel(){Category = category, Products = products};
            return View(productViewModel);
        }

        // localhost:5000/Product/Details
        public IActionResult Details(int id)
        {
            var product = new Product();
            product.Id = 1;
            product.Name = "Samsung Galaxy S20";
            product.Price = 7000;
            product.Description = "Nice smartphone";
            return View(product);
        }
    }
}
