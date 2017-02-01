using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckoutApp.Models;

namespace CheckoutApp.Repository
{
    //Simple value object to hold Production information
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
            return string.Equals(ProductId, other.ProductId) && string.Equals(ProductName, other.ProductName) && UnitPriceInCents == other.UnitPriceInCents;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ProductInfo) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (ProductId != null ? ProductId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ProductName != null ? ProductName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ UnitPriceInCents;
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"{nameof(ProductId)}: {ProductId}, {nameof(ProductName)}: {ProductName}, {nameof(UnitPriceInCents)}: {UnitPriceInCents}";
        }
    }
}
