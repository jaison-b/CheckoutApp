using CheckoutApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

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
            var csv = new CsvReader(new StreamReader(productsInputStream));
            csv.Configuration.TrimFields = true;
            csv.Configuration.TrimHeaders = true;
            csv.Configuration.RegisterClassMap<ProductInfoMapper>();
            csv.Configuration.WillThrowOnMissingField = false;
            return csv.GetRecords<ProductInfo>().ToList();
        }

        public ProductInfo GetProduct(string productId)
        {
            return
                _products.FirstOrDefault(
                    product => string.Equals(product.ProductId, productId, StringComparison.OrdinalIgnoreCase));
        }
    }

    internal sealed class ProductInfoMapper : CsvClassMap<ProductInfo>
    {
        public ProductInfoMapper()
        {
            Map(m => m.ProductId).Name("PRODUCT_ID");
            Map(m => m.ProductName).Name("PRODUCT_NAME");
            Map(m => m.UnitPriceInCents).ConvertUsing(row =>
            {
                var value = row.GetField("UNIT_PRICE");
                if (string.IsNullOrEmpty(value))
                {
                    throw new CsvMissingFieldException("Unit Price is required in row: " + row.Row);
                }
                return Convert.ToInt32(double.Parse(value) * 100); //convert to cents
            });
        }
    }
}