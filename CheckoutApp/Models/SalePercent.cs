using System;

namespace CheckoutApp.Models
{
    /// <summary>
    ///     Promotion for percent off on sale item
    /// </summary>
    public class SalePercent : Promotion
    {
        private readonly int _salePercent;
        private readonly int _thresholdQuantity;

        public SalePercent(IOrderItem orderItem, int thresholdQuantity, int salePercent) : base(orderItem)
        {
            if (salePercent < 0 || salePercent > 100)
                throw new ArgumentException("salePercent argument must be in [0, 100]");
            _thresholdQuantity = thresholdQuantity;
            _salePercent = salePercent;
        }

        public override int GetEffectivePrice(int quantity)
        {
            if (quantity < _thresholdQuantity)
                return base.GetEffectivePrice(quantity);
            var discountPrice = decimal.Multiply(UnitPrice(), decimal.Divide(_salePercent, 100));
            var salePrice = decimal.Subtract(UnitPrice(), discountPrice);
            return decimal.ToInt32(decimal.Multiply(salePrice, quantity));
        }

        protected bool Equals(SalePercent other)
        {
            return GetOrderItem().Equals(other.GetOrderItem()) && _thresholdQuantity == other._thresholdQuantity &&
                   _salePercent == other._salePercent;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((SalePercent) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = GetOrderItem().GetHashCode();
                hashCode = (hashCode * 397) ^ _thresholdQuantity;
                hashCode = (hashCode * 397) ^ _salePercent;
                return hashCode;
            }
        }
    }
}