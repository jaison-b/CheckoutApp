namespace CheckoutApp.Models
{
    public class ProductItem : IOrderItem
    {
        private readonly string _productId;
        private readonly string _productName;
        private readonly int _unitPriceInCents;

        public ProductItem(string productId, string productName, int unitPriceInCents)
        {
            _productId = productId;
            _productName = productName;
            _unitPriceInCents = unitPriceInCents;
        }

        public string ItemId()
        {
            return _productId;
        }

        public string Description()
        {
            return _productName;
        }

        public int UnitPrice()
        {
            return _unitPriceInCents;
        }

        public int GetEffectivePrice(int quantity)
        {
            return _unitPriceInCents * quantity;
        }

        public override string ToString()
        {
            return
                $"{nameof(_productId)}: {_productId}, {nameof(_productName)}: {_productName}, {nameof(_unitPriceInCents)}: {_unitPriceInCents}";
        }

        protected bool Equals(ProductItem other)
        {
            return string.Equals(_productId, other._productId) &&
                   string.Equals(_productName, other._productName) &&
                   _unitPriceInCents == other._unitPriceInCents;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ProductItem) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _productId?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (_productName?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ _unitPriceInCents;
                return hashCode;
            }
        }
    }
}