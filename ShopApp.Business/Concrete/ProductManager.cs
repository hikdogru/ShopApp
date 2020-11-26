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
            throw new System.NotImplementedException();
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
    }
}