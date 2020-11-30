using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entity;

namespace ShopApp.DataAccess.Concrete.EfCore
{
    public class EfCoreProductRepository : EfCoreGenericRepository<Product, ShopContext>, IProductRepository
    {
        public List<Product> GetPopularProducts()
        {
            return null;
        }

        public Product GetProductDetails(string url)
        {
            using (var context = new ShopContext())
            {
                var productCategory = context.Products.Where(p => p.Url == url)
                .Include(p => p.ProductCategories).ThenInclude(p => p.Category).FirstOrDefault();
                return productCategory;
            };
        }

        public List<Product> GetProductsByCategory(string categoryName)
        {
            using (var context = new ShopContext())
            {
                var products = context.Products.AsQueryable();
                if (!String.IsNullOrEmpty(categoryName))
                {
                    products = context.Products
                        .Include(p => p.ProductCategories)
                        .ThenInclude(p => p.Category)
                        .Where(p => p.ProductCategories.Any(p => p.Category.Url == categoryName));

                }
                return products.ToList();
            }
        }
    }
}
