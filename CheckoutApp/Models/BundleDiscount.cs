using System;

namespace CheckoutApp.Models
{
    /// <summary>
    ///     Promotion for bundle discounts
    ///     <para>For e.g Buy 3 for $2.00 type promotion</para>
    /// </summary>
    public class BundleDiscount : Promotion
    {
        private readonly int _bundleSize;
        private readonly int _discountedPriceInCents;

        public BundleDiscount(IOrderItem orderItem, int bundleSize, int discountedPriceInCents)
            : base(orderItem)
        {
            if (bundleSize < 2)
                throw new ArgumentException("Bundle size should be atleast '2' for pricing calculation");
            _bundleSize = bundleSize;
            _discountedPriceInCents = discountedPriceInCents;
        }

        public override int GetPrice(int quantity)
        {
            var bundles = quantity / _bundleSize;
            if (bundles < 1) return base.GetPrice(quantity);
            var remainder = quantity % _bundleSize;
            return bundles * _discountedPriceInCents + remainder * UnitPrice();
        }

        protected bool Equals(BundleDiscount other)
        {
            return GetOrderItem().Equals(other.GetOrderItem()) && _bundleSize == other._bundleSize &&
                   _discountedPriceInCents == other._discountedPriceInCents;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((BundleDiscount) obj);
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