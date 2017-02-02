using System;

namespace CheckoutApp.Repository
{
    //Represents type of Promotion
    public enum PromoType
    {
        SalePrice,
        SalePercent,
        BundleDiscount,
        AddOnPercent,
        AddOnUnit,
    }

    //Simple Value Object to hold Promotion Info
    public class PromotionInfo
    {
        public string ProductId { get; set; }
        public PromoType PromoType { get; set; }
        public DateTime StartDate { get; set; } = DateTime.MinValue;
        public DateTime EndDate { get; set; } = DateTime.MaxValue;
        public int EligibleQuantity { get; set; } = 0;
        public double PromoAmount { get; set; } = 0.00;

        protected bool Equals(PromotionInfo other)
        {
            return string.Equals(ProductId, other.ProductId) && PromoType == other.PromoType &&
                   StartDate.Equals(other.StartDate) && EndDate.Equals(other.EndDate) &&
                   EligibleQuantity == other.EligibleQuantity && PromoAmount.Equals(other.PromoAmount);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((PromotionInfo) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = ProductId?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (int) PromoType;
                hashCode = (hashCode * 397) ^ StartDate.GetHashCode();
                hashCode = (hashCode * 397) ^ EndDate.GetHashCode();
                hashCode = (hashCode * 397) ^ EligibleQuantity;
                hashCode = (hashCode * 397) ^ PromoAmount.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return
                $"{nameof(ProductId)}: {ProductId}, {nameof(PromoType)}: {PromoType}, {nameof(StartDate)}: {StartDate}, {nameof(EndDate)}: {EndDate}, {nameof(EligibleQuantity)}: {EligibleQuantity}, {nameof(PromoAmount)}: {PromoAmount}";
        }
    }
}