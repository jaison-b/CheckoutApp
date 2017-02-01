namespace CheckoutApp.Models
{
    public class SalePriceDecorator : PromotionDecorator
    {
        private readonly int _salePriceInCents;
        private readonly int _thresholdQuantity;

        public SalePriceDecorator(IOrderItem orderItem, int thresholdQuantity, int salePriceInCents)
            : base(orderItem)
        {
            _thresholdQuantity = thresholdQuantity;
            _salePriceInCents = salePriceInCents;
        }

        public override int PriceForQuantity(int quantity)
        {
            if (quantity < _thresholdQuantity)
                return base.PriceForQuantity(quantity);
            return _salePriceInCents * quantity;
        }

        protected bool Equals(SalePriceDecorator other)
        {
            return GetOrderItem().Equals(other.GetOrderItem()) && _thresholdQuantity == other._thresholdQuantity &&
                   _salePriceInCents == other._salePriceInCents;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((SalePriceDecorator) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = GetOrderItem().GetHashCode();
                hashCode = (hashCode * 397) ^ _thresholdQuantity;
                hashCode = (hashCode * 397) ^ _salePriceInCents;
                return hashCode;
            }
        }
    }
}