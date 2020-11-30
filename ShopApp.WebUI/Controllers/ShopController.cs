using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopApp.Business.Abstract;
using ShopApp.WebUI.ViewModels;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.Controllers
{
    public class ShopController : Controller
    {
        private IProductService _productService;
        public ShopController(IProductService productService)
        {
            this._productService = productService;
        }
        public IActionResult List(string category)
        {
            var productViewModel = new ProductListViewModel() { Products = _productService.GetProductsByCategory(category) };
            return View(productViewModel);
        }

        public IActionResult Details(string url)
        {
            if (url == null)
            {
                return NotFound();
            }
            var product = _productService.GetProductDetails(url);
            if (product == null)
            {
                return NotFound();
            }
            return View(new ProductDetailModel
            {
                Product = product,
                Categories = product.ProductCategories.Select(c => c.Category).ToList()
            });
        }
    }
}
