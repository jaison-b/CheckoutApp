using System;

namespace CheckoutApp.Models
{
    public class AddOnPercentPromoDecorator : PromotionDecorator
    {
        private readonly int _eligibleUnits;
        private readonly int _salePercent;

        public AddOnPercentPromoDecorator(IOrderItem orderItem, int eligibleUnits, int salePercent) : base(orderItem)
        {
            if (salePercent < 0 || salePercent > 100)
                throw new ArgumentException("salePercent argument must be in [0, 100]");
            if (eligibleUnits < 1)
                throw new ArgumentException("eligibleUnits must be atleast 1");
            _eligibleUnits = eligibleUnits;
            _salePercent = salePercent;
        }

        public override int PriceForQuantity(int quantity)
        {
            if (quantity < _eligibleUnits)
                return base.PriceForQuantity(quantity);
            //Calculation: Free_Percent/Total_Units = Discount Percent
            //For e.g Buy one get 50% off - so 50/2 Units = 25% discount overall
            var discountPercent = decimal.Divide(decimal.Divide(_salePercent, _eligibleUnits + 1), 100);
            var basePrice = base.PriceForQuantity(quantity);
            var discountAmount = decimal.ToInt32(decimal.Multiply(basePrice, discountPercent));
            return basePrice - discountAmount;
        }

        protected bool Equals(AddOnPercentPromoDecorator other)
        {
            return GetOrderItem().Equals(other.GetOrderItem()) && _eligibleUnits == other._eligibleUnits &&
                   _salePercent == other._salePercent;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((AddOnPercentPromoDecorator) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = GetOrderItem().GetHashCode();
                hashCode = (hashCode * 397) ^ _eligibleUnits;
                hashCode = (hashCode * 397) ^ _salePercent;
                return hashCode;
            }
        }
    }
}