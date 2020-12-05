using System.Collections.Generic;
using ShopApp.Entity;

namespace ShopApp.Business.Abstract
{
    public interface IProductService
    {
        Product GetById(int id);
        Product GetProductDetails(string url);
        List<Product> GetProductsByCategory(string categoryName,int page,int pageSize);
        List<Product> GetHomePageProducts();
        List<Product> GetSearchResult(string searchString);
        List<Product> GetAll();
        void Create(Product entity);
        void Update(Product entity);
        void Update(Product entity, int[] categoryIds);
        void Delete(Product entity);
        
        int GetCountByCategory(string category);
        Product GetByIdWithCategories(int productId);
        
    }
}