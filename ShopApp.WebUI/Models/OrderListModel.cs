using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopApp.Entity;

namespace ShopApp.WebUI.Models
{
    public class OrderListModel
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }

        public DateTime OrderDate { get; set; }
        public string UserId { get; set; }


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }

        public string Note { get; set; }
        public PaymentType PaymentType { get; set; }
        public OrderState OrderState { get; set; }
        public List<OrderItemModel> OrderItemModels { get; set; }

        public double TotalPrice()
        {
            return OrderItemModels.Sum(i => i.Price * i.Quantity);
        }
        
    }
    

}
