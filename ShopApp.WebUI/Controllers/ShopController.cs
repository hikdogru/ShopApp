﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopApp.Business.Abstract;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Concrete.EfCore;
using ShopApp.WebUI.Models;
using ShopApp.Entity;

namespace ShopApp.WebUI.Controllers
{
    public class ShopController : Controller
    {
        private IProductService _productService;
        public ShopController(IProductService productService)
        {
            this._productService = productService;
        }
        // localhost/Products/Cell-Phone?page=1
        [Route("Products")]
        [Route("Products/{category?}")]
        public IActionResult List(string category, int page = 1)
        {

            const int pageSize = 3; // Bir sayfada ne kadar ürünün gösterileceğini belirtir.
            var productViewModel = new ProductListViewModel()
            {
                PageInfo = new PageInfo()
                {
                    TotalItems = _productService.GetCountByCategory(category),
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    CurrentCategory = category
                },
                Products = _productService.GetProductsByCategory(category, page, pageSize)

            };
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


            //var productRecommends = ProductRecommendation(product.ProductId);
            //if (productRecommends==null)
            //{
            //    return NotFound();
            //}

            return View(new ProductDetailModel
            {
                Product = product,
                //ProductRecommends = productRecommends,
                Categories = product.ProductCategories.Select(c => c.Category).ToList()
            });
        }

        public IActionResult Search(string q)
        {
            var products = _productService.GetSearchResult(q);
            var productViewModel = new ProductListViewModel()
            {
                Products = products
            };
            return View(productViewModel);
        }

        //public List<Product> ProductRecommendation(int productId)
        //{
        //    var products = _productService.GetProductRecommendation(productId);
        //    return products;
        //}
    }
}
