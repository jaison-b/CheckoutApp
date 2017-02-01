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

        public int UnitPrice()
        {
            return _orderItem.UnitPrice();
        }

        public virtual int PriceForQuantity(int quantity)
        {
            return _orderItem.PriceForQuantity(quantity);
        }

        protected IOrderItem GetOrderItem()
        {
            return _orderItem;
        }

        //Enforce children to provide equals and hashcode since decorators will be stored as keys in map
        public abstract override bool Equals(object obj); 

        public abstract override int GetHashCode();
    }
}