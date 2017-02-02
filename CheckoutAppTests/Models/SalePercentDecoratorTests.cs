using System;
using CheckoutApp.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CheckoutAppTests.Models
{
    [TestClass()]
    public class SalePercentDecoratorTests
    {
        private readonly ProductItem _testItem = new ProductItem("1", "PASTA", 300);

        [TestMethod()]
        public void PriceForQuantity_OnEligibleQuantity_ShouldReturnDiscountedPricing()
        {
            var decorator = new SalePercent(_testItem, 1, 10);
            var expectedPriceInCents = 300 - 30; //pasta 3.00 with 10% off
            Assert.AreEqual(expectedPriceInCents, decorator.GetEffectivePrice(1));
        }

        [TestMethod]
        public void PriceForQuantity_OnEligibleQuantityAndFractionalDiscount_ShouldReturnRoundedDiscountPricing()
        {
            var decorator = new SalePercent(_testItem, 1, 33);
            var expectedPriceInCents = 300 - 99; //pasta 3.00 with 33% off
            Assert.AreEqual(expectedPriceInCents, decorator.GetEffectivePrice(1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid Sale Percent was allowed")]
        public void PriceForQuantity_OnInvalidSalePercent_ShouldThrowExcpeption()
        {
            var decorator = new SalePercent(_testItem, 1, 101);
        }

        [TestMethod]
        public void PriceForQuantity_OnNonEligibleQuantity_ShouldReturnActualPricing()
        {
            var decorator = new SalePercent(_testItem, 2, 10);
            var expectedPriceInCents = 300; //actual price of pasta 3.00
            Assert.AreEqual(expectedPriceInCents, decorator.GetEffectivePrice(1));
        }
    }
}