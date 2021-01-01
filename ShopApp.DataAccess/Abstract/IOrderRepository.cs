using System;
using System.Collections.Generic;
using System.Text;
using ShopApp.Entity;

namespace ShopApp.DataAccess.Abstract
{
    public interface IOrderRepository : IRepository<Order>
    {
        List<Order> GetOrders(string userId);
    }
}
