using System;

namespace CheckoutApp.Models
{
    public class SalePriceDecorator : PromotionDecorator
    {
        private readonly int _thresholdQuantity;
        private readonly int _salePriceInCents;

        public SalePriceDecorator(IOrderItem orderItem, int thresholdQuantity, int salePriceInCents)
            : base(orderItem)
        {
            _thresholdQuantity = thresholdQuantity;
            _salePriceInCents = salePriceInCents;
        }

        public override int PriceForQuantity(int quantity)
        {
            if (quantity < _thresholdQuantity)
            {
                return base.PriceForQuantity(quantity);
            }
            return _salePriceInCents * quantity;
        }
    }
}