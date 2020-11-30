using System.Collections.Generic;
using ShopApp.Entity;

namespace ShopApp.DataAccess.Abstract
{
    public interface IProductRepository:IRepository<Product>
    {
        Product GetProductDetails(string productName);
        List<Product> GetProductsByCategory(string categoryName);
        List<Product> GetPopularProducts();
        
    }
}