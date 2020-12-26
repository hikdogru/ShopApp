using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entity;

namespace ShopApp.DataAccess.Concrete.EfCore
{
    public class EfCoreCartRepository : EfCoreGenericRepository<Cart, ShopContext>, ICartRepository
    {
        public void ClearCart(int cartId)
        {
            using (var context = new ShopContext())
            {
                var cartItems = context.CartItems.Where(p => p.CartId == cartId).ToList();
                context.CartItems.RemoveRange(cartItems);
                context.SaveChanges();
            }
        }

        public void DeleteFromCart(int cartId, int productId)
        {
            using (var context = new ShopContext())
            {
                var entity = context.CartItems
                .Where(p => p.CartId == cartId && p.ProductId == productId)
                .FirstOrDefault();
                context.CartItems.Remove(entity);
                context.SaveChanges();

            }
        }

        public Cart GetCartByUserId(string userId)
        {
            using (var context = new ShopContext())
            {
                var cart = context.Carts.Include(i => i.CartItems)
                .ThenInclude(i => i.Product).FirstOrDefault(i => i.UserId == userId);
                return cart;
            }
        }

        public override void Update(Cart entity)
        {
            using (var context = new ShopContext())
            {
                context.Carts.Update(entity);
                context.SaveChanges();

            }
        }
    }
}