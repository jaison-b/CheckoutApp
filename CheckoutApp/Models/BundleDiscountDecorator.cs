using System;

namespace CheckoutApp.Models
{
    public class BundleDiscountDecorator : PromotionDecorator
    {
        private readonly int _bundleSize;
        private readonly int _discountedPriceInCents;

        public BundleDiscountDecorator(IOrderItem orderItem, int bundleSize, int discountedPriceInCents)
            : base(orderItem)
        {
            if (bundleSize < 2)
                throw new ArgumentException("Bundle size should be atleast '2' for pricing calculation");
            _bundleSize = bundleSize;
            _discountedPriceInCents = discountedPriceInCents;
        }

        public override int PriceForQuantity(int quantity)
        {
            var bundles = quantity / _bundleSize;
            if (bundles < 1) return base.PriceForQuantity(quantity);
            var remainder = quantity % _bundleSize;
            return bundles * _discountedPriceInCents + remainder * UnitPrice();
        }

        protected bool Equals(BundleDiscountDecorator other)
        {
            return GetOrderItem().Equals(other.GetOrderItem()) && _bundleSize == other._bundleSize &&
                   _discountedPriceInCents == other._discountedPriceInCents;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((BundleDiscountDecorator) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = GetOrderItem().GetHashCode();
                hashCode = (hashCode * 397) ^ _bundleSize;
                hashCode = (hashCode * 397) ^ _discountedPriceInCents;
                return hashCode;
            }
        }
    }
}