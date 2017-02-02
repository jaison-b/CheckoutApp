using System;
using System.IO;

namespace CheckoutApp
{
    public interface ICartFactory
    {
        /// <summary>
        ///     Create shopping cart containing order items
        /// </summary>
        /// <param name="ordersInputFile"></param>
        /// <param name="orderDate"></param>
        /// <returns><see cref="Cart"/></returns>
        Cart CreateCart(Stream ordersInputFile, DateTime orderDate);
    }
}