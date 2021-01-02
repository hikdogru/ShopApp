using System;
using System.Collections.Generic;
using System.Text;
using ShopApp.Business.Abstract;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entity;

namespace ShopApp.Business.Concrete
{
    public class OrderManager:IOrderService
    {
        private IOrderRepository _orderRepository;

        public OrderManager(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public void Create(Order order)
        {
            _orderRepository.Create(order);
        }

        public List<Order> GetOrders(string userId, bool isAdmin)
        {
            return _orderRepository.GetOrders(userId, isAdmin);
        }
    }

    
}
