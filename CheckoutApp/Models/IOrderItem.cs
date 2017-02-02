namespace CheckoutApp.Models
{
    public interface IOrderItem
    {
        string ItemId();
        string Description();
        int UnitPrice();

        /// <summary>
        ///     Returns effective price of the item after applying any available qualifying promotions
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns>Effective price in cents</returns>
        int GetEffectivePrice(int quantity);
    }
}