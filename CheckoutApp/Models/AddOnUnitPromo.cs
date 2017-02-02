using System;

namespace CheckoutApp.Models
{
    /// <summary>
    ///     Promotion to capture Add On Promotion deals
    ///     <para>For e.g Buy 1 Get 1 Free type offers</para>
    /// </summary>
    public class AddOnUnitPromo : Promotion
    {
        private readonly int _eligibleUnits;
        private readonly int _freeUnits;

        public AddOnUnitPromo(IOrderItem orderItem, int eligibleUnits, int freeUnits) : base(orderItem)
        {
            if (eligibleUnits < 1)
                throw new ArgumentException("eligibleUnits must be atleast 1");
            if (freeUnits < 1)
                throw new ArgumentException("freeUnits must be atleast 1");
            _eligibleUnits = eligibleUnits;
            _freeUnits = freeUnits;
        }

        public override int GetEffectivePrice(int quantity)
        {
            if (quantity < _eligibleUnits)
                return base.GetEffectivePrice(quantity);
            //Calcution: Free_Unit/Total_Units_To_Buy = discount Percent
            //For e.g Buy one get free - so 1 freeUnits for 2 Units : 1/2 = 0.5 (50%)
            var discountPercent = decimal.Divide(_freeUnits, _eligibleUnits + _freeUnits);
            var basePrice = base.GetEffectivePrice(quantity);
            var discountAmount = decimal.ToInt32(decimal.Multiply(basePrice, discountPercent));
            return basePrice - discountAmount;
        }

        protected bool Equals(AddOnUnitPromo other)
        {
            return GetOrderItem().Equals(other.GetOrderItem()) && _eligibleUnits == other._eligibleUnits &&
                   _freeUnits == other._freeUnits;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((AddOnUnitPromo) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = GetOrderItem().GetHashCode();
                hashCode = (hashCode * 397) ^ _eligibleUnits;
                hashCode = (hashCode * 397) ^ _freeUnits;
                return hashCode;
            }
        }
    }
}