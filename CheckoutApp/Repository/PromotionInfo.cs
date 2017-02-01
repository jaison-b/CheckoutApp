using System;

namespace CheckoutApp.Repository
{
    //Represents type of Promotion
    public enum PromoType
    {
        ReducePrice,
        BundleDiscount,
        AddOnPromo
    }

    //Units to consider for Promotion/Discount - For e.g discount by price or percent, in the future we can have loyalty points
    //that might affect the promo calculations differently
    public enum PromoUnit
    {
        Price,
        Percent
    }

    //Simple Value Object to hold Promotion Info
    public class PromotionInfo
    {
        public string ProductId { get; set; }
        public PromoType PromoType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int EligibleQuantity { get; set; }
        public double PromoAmount { get; set; }
        public PromoUnit PromoUnit { get; set; }

        protected bool Equals(PromotionInfo other)
        {
            return string.Equals(ProductId, other.ProductId) && PromoType == other.PromoType &&
                   StartDate.Equals(other.StartDate) && EndDate.Equals(other.EndDate) &&
                   PromoAmount.Equals(other.PromoAmount) && PromoUnit == other.PromoUnit;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((PromotionInfo) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = ProductId?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (int) PromoType;
                hashCode = (hashCode * 397) ^ StartDate.GetHashCode();
                hashCode = (hashCode * 397) ^ EndDate.GetHashCode();
                hashCode = (hashCode * 397) ^ PromoAmount.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) PromoUnit;
                return hashCode;
            }
        }

        public override string ToString()
        {
            return
                $"{nameof(ProductId)}: {ProductId}, {nameof(PromoType)}: {PromoType}, {nameof(StartDate)}: {StartDate}, {nameof(EndDate)}: {EndDate}, {nameof(PromoAmount)}: {PromoAmount}, {nameof(PromoUnit)}: {PromoUnit}";
        }
    }
}