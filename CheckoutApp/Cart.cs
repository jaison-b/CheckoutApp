using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using CheckoutApp.Models;
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
            Console.WriteAscii("Grocery Co", Color.LightGreen);
            Console.WriteLine("{0:MMM ddd d HH:mm yyyy}", DateTime.Now, Color.LightGreen);
            Console.WriteLine("");
            PrintLine();
            Console.WriteLine("| {0,-25} | {1,15} | {2,15} |", "Item", "Actual Price", "You Pay", Color.Magenta);
            PrintLine();
            var totalActualCost = decimal.Zero;
            foreach (var orderItem in _orderItems)
            {
                var effectiveCost = decimal.Divide(orderItem.Key.GetEffectivePrice(orderItem.Value), 100);
                var actualCost = decimal.Divide(orderItem.Key.UnitPrice() * orderItem.Value, 100);
                totalActualCost += actualCost;
                Console.WriteLine("| {0,-25} | {1,15:C} | {2,15:C} |", orderItem.Key.Description(),
                    actualCost, effectiveCost, Color.Cyan);
            }
            var totalEffectiveCost = decimal.Divide(GetTotalEffectivePrice(), 100);
            PrintLine();
            Console.WriteLine("| {0,-25}   {1,15}   {2,15:C} |", "Total Cost", "", totalEffectiveCost, Color.GreenYellow);
            PrintLine();
            Console.WriteLine("");
            Console.WriteLine("  {0,-25}   {1,15:C} Today", "You Saved",
                GetTotalSaved(totalActualCost, totalEffectiveCost), Color.GreenYellow);
            Console.WriteLine("");
            Console.WriteLine("**** Thank you for shopping with us ****", Color.LightGreen);
        }

        private decimal GetTotalSaved(decimal totalActualCost, decimal totalEffectiveCost)
        {
            var totalSaved = decimal.Subtract(totalActualCost, totalEffectiveCost);
            return totalSaved > 0 ? totalSaved : decimal.Zero;
        }

        private static void PrintLine()
        {
            Console.WriteLine(new string('-', 65));
        }

        public IReadOnlyDictionary<IOrderItem, int> GetOrderItems()
        {
            return new ReadOnlyDictionary<IOrderItem, int>(_orderItems);
        }

        /// <summary>
        ///     Returns Total effective price of shopping cart items
        /// </summary>
        /// <returns>Effective total price in cents</returns>
        public int GetTotalEffectivePrice()
        {
            return _orderItems.Sum(entry => entry.Key.GetEffectivePrice(entry.Value));
        }
    }
}