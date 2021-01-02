using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entity;

namespace ShopApp.DataAccess.Concrete.EfCore
{
    public class EfCoreOrderRepository : EfCoreGenericRepository<Order, ShopContext>, IOrderRepository
    {
        public List<Order> GetOrders(string userId, bool isAdmin)
        {
            using (var context = new ShopContext())
            {
                var orderList = context.Orders
                    .Include(i => i.OrderItems)
                    .ThenInclude(i => i.Product)
                    .AsQueryable();
                if (!string.IsNullOrEmpty(userId))
                {
                    if (isAdmin)
                    {
                        return orderList.ToList();
                    }
                    else
                    {
                        orderList = orderList.Where(i => i.UserId == userId);
                    }
                }

                return orderList.ToList();
            }
        }
    }
}
