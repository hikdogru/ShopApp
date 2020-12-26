using System;
using System.Collections.Generic;

namespace ShopApp.Entity
{
    public class Order
    {
        public int Id { get; set; }
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

        public string PaymentId { get; set; }
        public string ConversationId { get; set; }

        public PaymentType PaymentType { get; set; }
        public OrderState OrderState { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }

    public enum OrderState
    {
        // 0'dan başlıyor değerlerini belirtmezsek birer birer artıyor.
        Waiting,
        // 1
        Unpaid,
        //2
        Completed
    }

    public enum PaymentType
    {
        CreditCard,
        Eft
    }
}