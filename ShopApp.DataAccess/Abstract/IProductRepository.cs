using System.Collections.Generic;
using ShopApp.Entity;

namespace ShopApp.DataAccess.Abstract
{
    public interface IProductRepository:IRepository<Product>
    {
        Product GetProductDetails(string productName);
        List<Product> GetProductsByCategory(string categoryName,int page,int pageSize);
        List<Product> GetSearchResult(string searchString);
        List<Product> GetHomePageProducts();
        Product GetByIdWithCategories(int productId);
        int GetCountByCategory(string category);
        void Update(Product entity, int[] categoryIds);
    }
}