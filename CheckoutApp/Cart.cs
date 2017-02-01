using System.Collections.Generic;
using System.Collections.ObjectModel;
using CheckoutApp.Models;

namespace CheckoutApp
{
    public class Cart
    {
        private readonly IDictionary<IOrderItem, int> _orderItems;

        public Cart(IDictionary<IOrderItem, int> orderItems)
        {
            _orderItems = orderItems;
        }

        public void Checkout()
        {
        }

        public IReadOnlyDictionary<IOrderItem, int> GetOrderItems()
        {
            return new ReadOnlyDictionary<IOrderItem, int>(_orderItems);
        }
    }
}