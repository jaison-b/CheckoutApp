namespace CheckoutApp.Models
{
    /// <summary>
    ///     Promotion for reduced sale pricing for product items
    /// </summary>
    public class SalePrice : Promotion
    {
        private readonly int _salePriceInCents;
        private readonly int _thresholdQuantity;

        public SalePrice(IOrderItem orderItem, int thresholdQuantity, int salePriceInCents)
            : base(orderItem)
        {
            _thresholdQuantity = thresholdQuantity;
            _salePriceInCents = salePriceInCents;
        }

        public override int GetPrice(int quantity)
        {
            if (quantity < _thresholdQuantity)
                return base.GetPrice(quantity);
            return _salePriceInCents * quantity;
        }

        protected bool Equals(SalePrice other)
        {
            return GetOrderItem().Equals(other.GetOrderItem()) && _thresholdQuantity == other._thresholdQuantity &&
                   _salePriceInCents == other._salePriceInCents;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((SalePrice) obj);
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