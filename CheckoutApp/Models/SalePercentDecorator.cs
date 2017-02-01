﻿using System;

namespace CheckoutApp.Models
{
    public class SalePercentDecorator : PromotionDecorator
    {
        private readonly int _thresholdQuantity;
        private readonly int _salePercent;

        public SalePercentDecorator(IOrderItem orderItem, int thresholdQuantity, int salePercent) : base(orderItem)
        {
            if (salePercent < 0 || salePercent > 100)
            {
                throw new ArgumentException("salePercent argument must be in [0, 100]");
            }
            _thresholdQuantity = thresholdQuantity;
            _salePercent = salePercent;
        }

        public override int PriceForQuantity(int quantity)
        {
            if (quantity < _thresholdQuantity)
            {
                return base.PriceForQuantity(quantity);
            }
            var discountPrice = decimal.Multiply(UnitPrice(), decimal.Divide(_salePercent, 100));
            var salePrice = decimal.Subtract(UnitPrice(), discountPrice);
            return decimal.ToInt32(decimal.Multiply(salePrice, quantity));
        }
    }
}