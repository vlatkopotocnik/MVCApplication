using System.Collections.Generic;

namespace MetroAPP.Models.Proizvodi
{
    public class CartAndTotalPrice
    {
        public List<Cart> ListCart;
        public decimal TotalPrice;

        public CartAndTotalPrice(decimal totalPrice)
        {
            TotalPrice = totalPrice;
        }
    }
}