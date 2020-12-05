using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entity;

namespace ShopApp.DataAccess.Concrete.EfCore
{
    public class EfCoreCategoryRepository:EfCoreGenericRepository<Category, ShopContext>,ICategoryRepository
    {
        public List<Category> GetPopularCategories()
        {
            return null;
        }

        public Category GetByIdWithProducts(int categoryId)
        {
            using (var context = new ShopContext())
            {
                var list = context.Categories
                    .Where(c => c.CategoryId == categoryId)
                    .Include(c => c.ProductCategories)
                    .ThenInclude(c => c.Product).FirstOrDefault();

                return list;
            }
        }

        public void DeleteFromCategory(int productId, int categoryId)
        {
            using (var context = new ShopContext())
            {
                var query = "Delete From ProductCategory Where ProductId=@p0 and CategoryId=@p1";
                context.Database.ExecuteSqlRaw(query, productId, categoryId);
            }
        }
    }
}
