using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ShopApp.Business.Abstract;
using ShopApp.Entity;
using ShopApp.WebUI.Identity;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.Controllers
{
    public class OrderController : Controller
    {
        private IOrderService _orderService;
        private UserManager<User> _userManager;

        public OrderController(IOrderService orderService, UserManager<User> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetOrders()
        {
            string userId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("admin"); 
            var orderList = _orderService.GetOrders(userId,isAdmin);
            var orderListModel = new List<OrderListModel>();
            OrderListModel orderModel;
            foreach (var order in orderList)
            {
                orderModel = new OrderListModel()
                {
                    OrderId = order.Id,
                    OrderNumber = order.OrderNumber,
                    OrderDate = order.OrderDate,
                    Note = order.Note,
                    Phone = order.Phone,
                    FirstName = order.FirstName,
                    LastName = order.LastName,
                    Email = order.Email,
                    City = order.City,
                    Address = order.Address,
                    OrderItems = order.OrderItems.Select(i => new OrderItemModel()
                    {
                        OrderItemId = i.Id,
                        Name = i.Product.Name,
                        ImageUrl = i.Product.ImageUrl,
                        Price = i.Price,
                        Quantity = i.Quantity
                    }).ToList()

                };

                orderListModel.Add(orderModel);
            }

            return View("Orders", orderListModel);
        }
    }
}
