using System.Collections.Generic;
using System.Linq;

namespace ShopApp.WebUI.Models
{
    public class CartModel
    {
        public int CartId { get; set; }
        public List<CartItemModel> CartItems { get; set; }
        public double TotalPrice(){
            var totalPrice = CartItems.Sum(i=>i.Price * i.Quantity);
            return totalPrice;
            
        }
    }

    public class CartItemModel
    {
        public int CartItemId { get; set; }
        
        public int ProductId { get; set; }
        
        public string Name { get; set; }
        
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        
        
        
    }
}