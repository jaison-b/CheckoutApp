namespace CheckoutApp.Models
{
    public abstract class PromotionDecorator : IOrderItem
    {
        private readonly IOrderItem _orderItem;

        protected PromotionDecorator(IOrderItem orderItem)
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

        public abstract int UnitPrice();
        public abstract int PriceForQuantity(int quantity);
    }
}