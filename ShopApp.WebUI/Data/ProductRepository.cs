using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.Data
{
    public static class ProductRepository
    {
        private static List<Product> _products = null;

        static ProductRepository()
        {
            _products = new List<Product>()
            {
                new Product() {ProductId = 1, Name = "Galaxy S10", Price = 600, Description = "Good phone", IsApproved = false, ImageUrl = "m_samsung-galaxy-note-20-ultra-1.png", CategoryId = 1},
                new Product() {ProductId = 2, Name = "Galaxy S20", Price = 800, Description = "Very nice phone", IsApproved = true, ImageUrl = "m_oneplus-8-pro-256gb-14.png", CategoryId = 1},
                new Product() {ProductId = 3, Name = "Xioami Mi 10", Price = 800, Description = "Incredible phone", IsApproved = true, ImageUrl = "m_samsung-galaxy-s20-ultra-1.png", CategoryId = 1},
                new Product() {ProductId = 4, Name = "Huawei P40 Pro", Price = 800, Description = "Incredible phone", IsApproved = true, ImageUrl = "m_huawei-p40-pro-5.png", CategoryId = 1},


                new Product() {ProductId = 5, Name = "Lenovo", Price = 1000, Description = "Good laptop", IsApproved = false, ImageUrl = "m_samsung-galaxy-note-20-ultra-1.png", CategoryId = 3},
                new Product() {ProductId = 6, Name = "Apple", Price = 700, Description = "Very nice laptop", IsApproved = true, ImageUrl = "m_oneplus-8-pro-256gb-14.png", CategoryId = 3},
                new Product() {ProductId = 7, Name = "Hp", Price = 600, Description = "Incredible laptop", IsApproved = true, ImageUrl = "m_samsung-galaxy-s20-ultra-1.png", CategoryId = 2},
                new Product() {ProductId = 8, Name = "Asus", Price = 900, Description = "Incredible laptop", IsApproved = true, ImageUrl = "m_huawei-p40-pro-5.png", CategoryId = 2}
            };

        }

        public static List<Product> Products
        {
            get
            {
                return _products;

            }
        }

        public static void AddProduct(Product product)
        {
            _products.Add(product);
        }

        public static Product GetProductById(int productId)
        {
            var product = _products.FirstOrDefault(p => p.ProductId == productId);
            return product;
        }

        public static void EditProduct(Product product)
        {
            // var p = _products.Find(p => p.ProductId == product.ProductId);
            foreach (var p in _products)
            {
                if (p.ProductId == product.ProductId)
                {
                    p.Name = product.Name;
                    p.Price = product.Price;
                    p.Description = product.Description;
                    p.ImageUrl = product.ImageUrl;
                    p.CategoryId = product.CategoryId;
                }
            }
        }

        public static void Delete(int productId)
        {
            var product = GetProductById(productId);
            if (product !=null)
            {
            _products.Remove(product);
            }
        }
    }
}
