using CheckoutApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutApp.Repository
{
    public interface IProductRepository
    {
        //Why doesn't C# have Optional return types?
        /// <summary>
        /// Find the Product for given product id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>
        ///  Valid ProductInfo or Null if not values found
        /// </returns>
        ProductInfo GetProduct(string productId);
    }

    public class ProductRepository : IProductRepository
    {
        private IList<ProductInfo> _products;

        public ProductRepository(Stream productsInputStream)
        {
            _products = ParseProductsInputStream(productsInputStream);
        }

        private static IList<ProductInfo> ParseProductsInputStream(Stream productsInputStream)
        {
            return new List<ProductInfo>();
        }

        public ProductInfo GetProduct(string productId)
        {
            throw new NotImplementedException();
        }
    }
}