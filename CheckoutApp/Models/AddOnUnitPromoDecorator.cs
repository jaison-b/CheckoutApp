using System;

namespace CheckoutApp.Models
{
    public class AddOnUnitPromoDecorator : PromotionDecorator
    {
        private readonly int _eligibleUnits;
        private readonly int _freeUnits;

        public AddOnUnitPromoDecorator(IOrderItem orderItem, int eligibleUnits, int freeUnits) : base(orderItem)
        {
            if (eligibleUnits < 1)
                throw new ArgumentException("eligibleUnits must be atleast 1");
            if (freeUnits < 1)
                throw new ArgumentException("freeUnits must be atleast 1");
            _eligibleUnits = eligibleUnits;
            _freeUnits = freeUnits;
        }

        public override int PriceForQuantity(int quantity)
        {
            if (quantity < _eligibleUnits)
                return base.PriceForQuantity(quantity);
            //Calcution: Free_Unit/Total_Units_To_Buy = discount Percent
            //For e.g Buy one get free - so 1 freeUnits for 2 Units : 1/2 = 0.5 (50%)
            var discountPercent = decimal.Divide(_freeUnits, (_eligibleUnits + _freeUnits));
            var basePrice = base.PriceForQuantity(quantity);
            var discountAmount = decimal.ToInt32(decimal.Multiply(basePrice, discountPercent));
            return basePrice - discountAmount;
        }
    }
}