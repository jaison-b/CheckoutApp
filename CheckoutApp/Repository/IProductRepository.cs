namespace CheckoutApp.Repository
{
    public interface IProductRepository
    {
        /// <summary>
        ///     Find the Product for given product id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>
        ///     Valid ProductInfo or Null if no value found
        /// </returns>
        ProductInfo GetProduct(string productId);
    }
}