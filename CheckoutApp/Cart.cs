using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using CheckoutApp.Models;
using Colorful;
using Console = Colorful.Console;

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
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.WriteAscii("Grocery Co", Color.SteelBlue);
            PrintLine();
            Console.WriteLine("| {0,-25} | {1,10} |", "Item", "Cost", Color.Magenta);
            PrintLine();
            foreach (var orderItem in _orderItems)
            {
                var cost = decimal.Divide(orderItem.Key.PriceForQuantity(orderItem.Value), 100);
                PrintCheckoutItem(orderItem.Key.Description(), cost, Color.Cyan);
            }
            var totalCost = decimal.Divide(GetTotalPriceInCents(), 100);
            PrintLine();
            PrintCheckoutItem("Total Cost", totalCost, Color.GreenYellow);
            PrintLine();
            Console.WriteLine("**** Thank you for shopping with us ****", Color.SteelBlue);
        }

        private static void PrintLine()
        {
            Console.WriteLine(new string('-', 42));
        }

        private static void PrintCheckoutItem(string item, decimal cost, Color color)
        {
            Console.WriteLine("| {0,-25} | {1,10:C} |", item, cost, color);
        }

        public IReadOnlyDictionary<IOrderItem, int> GetOrderItems()
        {
            return new ReadOnlyDictionary<IOrderItem, int>(_orderItems);
        }

        public int GetTotalPriceInCents()
        {
            return _orderItems.Sum(entry => entry.Key.PriceForQuantity(entry.Value));
        }
    }
}