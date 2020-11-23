using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
