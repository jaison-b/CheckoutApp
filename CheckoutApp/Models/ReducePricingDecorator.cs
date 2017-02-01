using System;

namespace CheckoutApp.Models
{
    public class ReducePricingDecorator : PromotionDecorator
    {
        private readonly int _thresholdQuantity;
        private readonly int _reducedUnitPriceInCents;

        public ReducePricingDecorator(IOrderItem orderItem, int thresholdQuantity, int reducedUnitPriceInCents)
            : base(orderItem)
        {
            _thresholdQuantity = thresholdQuantity;
            _reducedUnitPriceInCents = reducedUnitPriceInCents;
        }

        public override int PriceForQuantity(int quantity)
        {
            if (quantity < _thresholdQuantity)
            {
                return base.PriceForQuantity(quantity);
            }
            return _reducedUnitPriceInCents * quantity;
        }
    }
}