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

        public int GetCountByCategory(string category)
        {
            using (var context = new ShopContext())
            {
                var products = context.Products.Where(p => p.IsApproved).AsQueryable();
                if (!String.IsNullOrEmpty(category))
                {
                    products = context.Products
                        .Include(p => p.ProductCategories)
                        .ThenInclude(p => p.Category)
                        .Where(p => p.ProductCategories.Any(p => p.Category.Url == category));

                }
                return products.Count();
            }
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

        public List<Product> GetProductsByCategory(string categoryName, int page, int pageSize)
        {
            using (var context = new ShopContext())
            {
                var products = context.Products.Where(p => p.IsApproved).AsQueryable();
                if (!String.IsNullOrEmpty(categoryName))
                {
                    products = context.Products
                        .Include(p => p.ProductCategories)
                        .ThenInclude(p => p.Category)
                        .Where(p => p.ProductCategories.Any(p => p.Category.Url == categoryName));

                }
                return products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        public List<Product> GetSearchResult(string searchString)
        {
            using (var context = new ShopContext())
            {
                var products = context
                    .Products
                    .Where(p => p.IsApproved && (p.Name.Contains(searchString)) || (p.Description.Contains(searchString)))
                    .AsQueryable();
                return products.ToList();
            }
        }

        public List<Product> GetHomePageProducts()
        {
            using (var context = new ShopContext())
            {
                var homePageProducts = context.Products.Where(p => p.IsApproved && p.IsHome);
                return homePageProducts.ToList();
            }
        }
    }
}
