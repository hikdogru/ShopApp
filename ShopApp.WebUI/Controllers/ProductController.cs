using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            { ProductId = 2, Name = "Samsung Galaxy S20 Ultra", Price = 1000, Description = "Very nice telephone." };
            ViewData["Product"] = product;
            ViewData["Category"] = "Smart Phones";

            ViewBag.Category = "Smart Phones";
            ViewBag.Product = product;
            return View(product);
        }

        // localhost:5000/Product/List
        // string varsayılan olarak nullable olduğu için ? işareti koymadık.
        public IActionResult List(int? id, string q)
        {
            var products = ProductRepository.Products;
            if (id != null)
            {
                products = products.Where(p => p.CategoryId == id).ToList();
            }

            if (!String.IsNullOrEmpty(q))
            {
                // Ürün ismi ve açıklamasına göre arama yapılıyor.
                products = products.Where(p => p.Name.ToLower().Contains(q.ToLower()) || p.Description.ToLower().Contains(q.ToLower())).ToList();
            }
            var productViewModel = new ProductViewModel() { Products = products };
            return View(productViewModel);
        }

        // localhost:5000/Product/Details
        public IActionResult Details(int id)
        {
            var product = ProductRepository.GetProductById(id);
            return View(product);
        }
        [HttpGet] // Varsayılan olarak Get View ekranlarını karşımıza getirir.
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(CategoryRepository.Categories, "CategoryId", "Name");
            return View(new Product());
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                ProductRepository.AddProduct(product);
                // Ürün listesine yönlendirir.
                return RedirectToAction("List");
            }
            ViewBag.Categories = new SelectList(CategoryRepository.Categories, "CategoryId", "Name");

            // Bilgiler yanlış girilse bile form tekrar açılır.
            return View(product);


        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Categories = new SelectList(CategoryRepository.Categories, "CategoryId", "Name");
            return View(ProductRepository.GetProductById(id));
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            ProductRepository.EditProduct(product);
            return RedirectToAction("List");
        }

        [HttpPost]
        public IActionResult Delete(int productId)
        {
            ProductRepository.Delete(productId);

            return RedirectToAction("List");
        }
    }
}
