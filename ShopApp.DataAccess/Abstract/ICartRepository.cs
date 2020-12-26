using ShopApp.Entity;

namespace ShopApp.DataAccess.Abstract
{
    public interface ICartRepository : IRepository<Cart>
    {
        Cart GetCartByUserId(string userId);
        void DeleteFromCart(int cartId, int productId);
        void ClearCart(int cartId);
    }
}