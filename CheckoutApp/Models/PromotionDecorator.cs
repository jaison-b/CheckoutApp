namespace CheckoutApp.Models
{
    public class PromotionDecorator : IOrderItem
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

        public int UnitPrice()
        {
            return _orderItem.UnitPrice();
        }

        public virtual int PriceForQuantity(int quantity)
        {
            return _orderItem.PriceForQuantity(quantity);
        }
    }
}