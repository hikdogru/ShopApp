using System.Collections.Generic;
using ShopApp.Entity;

namespace ShopApp.Business.Abstract
{
    public interface IProductService:IValidator<Product>
    {
        Product GetById(int id);
        Product GetProductDetails(string url);
        List<Product> GetProductsByCategory(string categoryName,int page,int pageSize);
        List<Product> GetHomePageProducts();
        List<Product> GetSearchResult(string searchString);
        List<Product> GetAll();
        bool Create(Product entity);
        bool Update(Product entity);
        bool Update(Product entity, int[] categoryIds);
        void Delete(Product entity);
        
        int GetCountByCategory(string category);
        Product GetByIdWithCategories(int productId);
        
    }
}