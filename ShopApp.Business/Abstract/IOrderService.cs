using System;
using System.Collections.Generic;
using System.Text;
using ShopApp.Entity;

namespace ShopApp.Business.Abstract
{
    public interface IOrderService
    {
        void Create(Order order);
        List<Order> GetOrders(string userId);
    }
}
