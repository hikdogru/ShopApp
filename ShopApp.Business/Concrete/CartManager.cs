using ShopApp.Business.Abstract;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entity;

namespace ShopApp.Business.Concrete
{
    public class CartManager : ICartService
    {
        private ICartRepository _cartRepository;
        public CartManager(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public void AddToCart(string userId, int productId, int quantity)
        {
            var cart = GetCartByUserId(userId);
            if (cart != null)
            {
                // eklemek istenen ürün sepette var mı? (güncelleme)
                var index = cart.CartItems.FindIndex(i => i.ProductId == productId);
                if (index < 0)
                {
                    cart.CartItems.Add(new CartItem()
                    {
                        ProductId = productId,
                        Quantity = quantity,
                        CartId = cart.Id
                    });
                }
                else
                {
                    cart.CartItems[index].Quantity += quantity;
                }
                _cartRepository.Update(cart);
            }
        }

        public void ClearCart(int cartId)
        {
            _cartRepository.ClearCart(cartId);
        }

        public void DeleteFromCart(string userId, int productId)
        {
            var cart = GetCartByUserId(userId);
            if (cart != null)
            {
                _cartRepository.DeleteFromCart(cart.Id,productId);
            }
        }

        public Cart GetCartByUserId(string userId)
        {
            var cart = _cartRepository.GetCartByUserId(userId);
            return cart;
        }

        public void InitializeCart(string userId)
        {
            _cartRepository.Create(new Cart() { UserId = userId });
        }
    }
}