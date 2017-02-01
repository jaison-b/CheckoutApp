﻿using CheckoutApp.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CheckoutAppTests.Models
{
    [TestClass()]
    public class ReducePricingDecoratorTests
    {
        private readonly ProductItem _testItem = new ProductItem("1", "APPLE", 75);
        [TestMethod()]
        public void PriceForQuantity_OnItemQuantityLessThanEligibleThreshold_ShouldReturnActualPricing()
        {
            var decorator = new ReducePricingDecorator(_testItem, 3, 50);
            var expectedPricingInCents = 75 * 2; //2 apples in actual price
            Assert.AreEqual(expectedPricingInCents, decorator.PriceForQuantity(2));
        }

        [TestMethod()]
        public void PriceForQuantity_OnItemQuantityAboveEligibleThreshold_ShouldReturnDiscountedPricing()
        {
            var decorator = new ReducePricingDecorator(_testItem, 1, 50);
            var expectedPricingInCents = 50 * 2; //2 apples in discount price
            Assert.AreEqual(expectedPricingInCents, decorator.PriceForQuantity(2));
        }

        [TestMethod]
        public void PriceForQuantity_OnItemQuantitySameAsThreshold_ShouldReturnDiscoutedPricing()
        {
            var decorator = new ReducePricingDecorator(_testItem, 1, 50);
            var expectedPricingInCents = 50 * 1; //1 apple in discount price
            Assert.AreEqual(expectedPricingInCents, decorator.PriceForQuantity(1));
        }
    }
}