using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();
        }

        // Post
        [HttpPost]
        public IActionResult CreateProduct(ProductModel model)
        {
            var entity = new Product()
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                ImageUrl = model.ImageUrl,
                Url = model.Url
            };
            _productService.Create(entity);
            // View kullansaydık ViewData kullanabilirdik.
            var msj = new AlertMessage() { AlertType = "success", Message = $"The product name {entity.Name} added." };
            // TempData için mesajı Json'a çevirdik.
            TempData["messsage"] = JsonConvert.SerializeObject(msj);
            return RedirectToAction("ProductList");
        }

        // Get
        [HttpGet]
        public IActionResult EditProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _productService.GetById((int)id);
            if (entity == null)
            {
                return NotFound();
            }

            var model = new ProductModel()
            {
                ProductId = entity.ProductId,
                Name = entity.Name,
                Url = entity.Url,
                Description = entity.Description,
                Price = entity.Price,
                ImageUrl = entity.ImageUrl,

            };

            return View(model);
        }

        // Post
        [HttpPost]
        public IActionResult EditProduct(ProductModel model)
        {
            var entity = _productService.GetById(model.ProductId);
            if (entity == null)
            {
                NotFound();
            }

            entity.ProductId = model.ProductId;
            entity.Name = model.Name;
            entity.Url = model.Url;
            entity.Description = model.Description;
            entity.Price = model.Price;
            entity.ImageUrl = model.ImageUrl;

            var msg = new AlertMessage()
            {
                AlertType = "success",
                Message = $"The product name {entity.Name} modified."
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);
            _productService.Update(entity);
            return RedirectToAction("ProductList");
        }

        public IActionResult DeleteProduct(int productId)
        {
            var entity = _productService.GetById(productId);
            if (entity != null)
            {
                _productService.Delete(entity);
            }

            var msj = new AlertMessage()
            {
                AlertType = "danger",
                Message = $"The product name {entity.Name} deleted!"
            };
            TempData["message"] = JsonConvert.SerializeObject(msj);
            return RedirectToAction("ProductList");
        }

    }
}
