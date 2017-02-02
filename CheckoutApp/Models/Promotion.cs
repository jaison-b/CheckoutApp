namespace CheckoutApp.Models
{
    /// <summary>
    ///     Base Class for Promotion Implementations
    ///     <para>
    ///         A Decorator Pattern is used to implement Promotions, so <see cref="IOrderItem" />
    ///         get "decorated" by <see cref="Promotion" /> to help calculate the final effective price for
    ///         item.
    ///     </para>
    /// </summary>
    public abstract class Promotion : IOrderItem
    {
        private readonly IOrderItem _orderItem;

        protected Promotion(IOrderItem orderItem)
        {
            _orderItem = orderItem;
        }

        public string ItemId()
        {
            return _orderItem.ItemId();
        }

        public string Description()
        {
            return _orderItem.Description();
        }

        public int UnitPrice()
        {
            return _orderItem.UnitPrice();
        }

        public virtual int GetPrice(int quantity)
        {
            return _orderItem.GetPrice(quantity);
        }

        protected IOrderItem GetOrderItem()
        {
            return _orderItem;
        }

        //Enforce children to provide equals and hashcode
        public abstract override bool Equals(object obj);

        public abstract override int GetHashCode();
    }
}