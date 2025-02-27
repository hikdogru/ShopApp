﻿using System;
using System.Collections.Generic;
using System.Text;
using ShopApp.Entity;

namespace ShopApp.Business.Abstract
{
    public interface ICategoryService:IValidator<Category>
    {
        Category GetById(int id);
        List<Category> GetAll();
        void Create(Category entity);
        void Update(Category entity);
        void Delete(Category entity);
        Category GetByIdWithProducts(int categoryId);
        void DeleteFromCategory(int productId, int categoryId);
    }
}
