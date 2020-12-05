using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShopApp.Business.Abstract;
using ShopApp.Entity;
using ShopApp.WebUI.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ShopApp.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;
        

        public AdminController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public IActionResult ProductList()
        {
            return View(new ProductListViewModel()
            {
                Products = _productService.GetAll()
            });
        }

        // Get metodu
        [HttpGet]
        public IActionResult ProductCreate()
        {
            return View();
        }

        // Post metodu
        [HttpPost]
        public IActionResult ProductCreate(ProductModel model)
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
        public IActionResult ProductEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _productService.GetByIdWithCategories((int)id);
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
                SelectedCategories = entity.ProductCategories.Select(p=>p.Category).ToList()
            };

            ViewBag.Categories = _categoryService.GetAll();

            return View(model);
        }

        // Post
        [HttpPost]
        public IActionResult ProductEdit(ProductModel model, int[] categoryIds)
        {
            var entity = _productService.GetById(model.ProductId);
            if (entity == null)
            {
                return NotFound();
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
            _productService.Update(entity, categoryIds);
           
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

        public IActionResult CategoryList()
        {
            var categoryList = new CategoryListViewModel() { Categories = _categoryService.GetAll()};
            return View(categoryList);
        }
        // Get metodu
        [HttpGet]
        public IActionResult CategoryCreate()
        {
            return View();
        }
        // Post metodu
        [HttpPost]
        public IActionResult CategoryCreate(CategoryModel model)
        {
            var entity = new Category()
            {
                Name = model.Name,
                Url = model.Url
            };
            _categoryService.Create(entity);
            var msg = new AlertMessage()
            {
                AlertType = "success",
                Message = $"The category name {entity.Name} added."
            };

            TempData["message"] = JsonConvert.SerializeObject(msg);
            return RedirectToAction("CategoryList");
        }

        // Get
        [HttpGet]
        public IActionResult CategoryEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _categoryService.GetByIdWithProducts((int)id);
            if (entity == null)
            {
                return NotFound();
            }

            var model = new CategoryModel()
            {
                CategoryId = entity.CategoryId,
                Name = entity.Name,
                Url = entity.Url,
                Products = entity.ProductCategories.Select(p=>p.Product).ToList()
            };

            return View(model);

        }

        // Post metodu
        [HttpPost]
        public IActionResult CategoryEdit(CategoryModel model)
        {
            var entity = _categoryService.GetById(model.CategoryId);
            if (entity == null)
            {
                return NotFound();
            }

            entity.Name = model.Name;
            entity.Url = model.Url;
            _categoryService.Update(entity);

            var msg = new AlertMessage()
            {
                AlertType = "success",
                Message = $" Category name {entity.Name} modified."
            };

            TempData["message"] = JsonConvert.SerializeObject(msg);

            return RedirectToAction("CategoryList");

        }

        public IActionResult DeleteCategory(int categoryId)
        {
            var entity = _categoryService.GetById(categoryId);
            if (entity != null)
            {
                _categoryService.Delete(entity);
            }

            var msj = new AlertMessage()
            {
                AlertType = "danger",
                Message = $"The category name {entity.Name} deleted!"
            };
            TempData["message"] = JsonConvert.SerializeObject(msj);
            return RedirectToAction("CategoryList");
        }
        [HttpPost]
        public IActionResult DeleteFromCategory(int productId, int categoryId)
        {
            _categoryService.DeleteFromCategory(productId, categoryId);
            return Redirect("/Admin/Categories/"+categoryId);
        }

    }
}
