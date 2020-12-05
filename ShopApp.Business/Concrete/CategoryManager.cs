using System;
using System.Collections.Generic;
using System.Text;
using ShopApp.Business.Abstract;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entity;

namespace ShopApp.Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        ICategoryRepository _categoryRepository;
        public CategoryManager(ICategoryRepository categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }
        public void Create(Category entity)
        {
            _categoryRepository.Create(entity);
        }

        public void Delete(Category entity)
        {
            _categoryRepository.Delete(entity);
        }

        public Category GetByIdWithProducts(int categoryId)
        {
            return _categoryRepository.GetByIdWithProducts(categoryId);
        }

        public void DeleteFromCategory(int productId, int categoryId)
        {
            _categoryRepository.DeleteFromCategory(productId, categoryId);
        }

        public List<Category> GetAll()
        {
            var categoryList = _categoryRepository.GetAll();
            return categoryList;
        }

        public Category GetById(int id)
        {
            return _categoryRepository.GetById(id);
        }

        public void Update(Category entity)
        {
            _categoryRepository.Update(entity);
        }
    }
}
