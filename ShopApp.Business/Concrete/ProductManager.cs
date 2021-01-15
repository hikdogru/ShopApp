using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using ShopApp.Business.Abstract;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Concrete.EfCore;
using ShopApp.Entity;

namespace ShopApp.Business.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductRepository _productRepository;

        public ProductManager(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }


        public Product GetById(int id)
        {
            var product = _productRepository.GetById(id);
            return product;
        }

        public List<Product> GetProductsByCategory(string categoryName, int page, int pageSize)
        {
            var products = _productRepository.GetProductsByCategory(categoryName, page, pageSize);
            return products;
        }

        public List<Product> GetSearchResult(string searchString)
        {
            return _productRepository.GetSearchResult(searchString);
        }

        public List<Product> GetAll()
        {
            var productList = _productRepository.GetAll();
            return productList;
        }

        public void Delete(Product entity)
        {
            // İş kuralları uygula
            _productRepository.Delete(entity);
        }

        public int GetCountByCategory(string category)
        {
            return _productRepository.GetCountByCategory(category);
        }

        public Product GetByIdWithCategories(int productId)
        {
            return _productRepository.GetByIdWithCategories(productId);
        }

        //public List<Product> GetProductRecommendation(int productId)
        //{
        //    return _productRepository.GetProductRecommendation(productId);
        //}

        public bool Update(Product entity)
        {
            if (Validation(entity))
            {
                _productRepository.Update(entity);
                return true;
            }

            return false;
        }

        public bool Update(Product entity, int[] categoryIds)
        {
            if (Validation(entity))
            {
                if (categoryIds.Length == 0)
                {
                    ErrorMessage += "You must choose a category!";
                    return false;
                }
                _productRepository.Update(entity, categoryIds);
                return true;
            }

            return false;
        }

        public bool Create(Product entity)
        {
            // İş kuralları uygula
            if (Validation(entity))
            {
                _productRepository.Create(entity);
                return true;
            }

            return false;
        }

        public Product GetProductDetails(string url)
        {
            var productDetails = _productRepository.GetProductDetails(url);
            return productDetails;
        }

        public List<Product> GetHomePageProducts()
        {
            return _productRepository.GetHomePageProducts();
        }

        public string ErrorMessage { get; set; }
        public bool Validation(Product entity)
        {
            var isValid = true;
            if (string.IsNullOrEmpty(entity.Name))
            {
                ErrorMessage += "You must enter product name!\n";
                isValid = false;
            }

            if (entity.Price < 0)
            {
                ErrorMessage += "The product price is not negative!\n";
                isValid = false;
            }

            return isValid;
        }

        
    }
}