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

        public List<Product> GetProductsByCategory(string categoryName)
        {
            var products = _productRepository.GetProductsByCategory(categoryName);
            return products;
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

        public void Update(Product entity)
        {
            throw new System.NotImplementedException();
        }

        public void Create(Product entity)
        {
            // İş kuralları uygula
            _productRepository.Create(entity);
        }

        public Product GetProductDetails(string url)
        {
            var productDetails = _productRepository.GetProductDetails(url);
            return productDetails;
        }
    }
}