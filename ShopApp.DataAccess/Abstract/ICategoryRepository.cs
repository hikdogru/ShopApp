﻿using System;
using System.Collections.Generic;
using System.Text;
using ShopApp.Entity;

namespace ShopApp.DataAccess.Abstract
{
    public interface ICategoryRepository:IRepository<Category>
    {
        List<Category> GetPopularCategories();
        Category GetByIdWithProducts(int categoryId);
        void DeleteFromCategory(int productId, int categoryId);
    }
}
