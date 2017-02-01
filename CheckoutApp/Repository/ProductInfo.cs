using CheckoutApp.Models;

namespace CheckoutApp.Repository
{
    //Simple value object to hold Product information
    public class ProductInfo
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int UnitPriceInCents { get; set; }

        public ProductItem ToProductItem()
        {
            return new ProductItem(ProductId, ProductName, UnitPriceInCents);
        }

        protected bool Equals(ProductInfo other)
        {
            return string.Equals(ProductId, other.ProductId) &&
                   string.Equals(ProductName, other.ProductName) &&
                   UnitPriceInCents == other.UnitPriceInCents;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ProductInfo) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = ProductId?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (ProductName?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ UnitPriceInCents;
                return hashCode;
            }
        }

        public override string ToString()
        {
            return
                $"{nameof(ProductId)}: {ProductId}, {nameof(ProductName)}: {ProductName}, {nameof(UnitPriceInCents)}: {UnitPriceInCents}";
        }
    }
}