using System;
using CheckoutApp.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CheckoutAppTests.Models
{
    [TestClass]
    public class AddOnPercentPromoDecoratorTests
    {
        private readonly ProductItem _productItem = new ProductItem("1", "BOOK", 3000);

        [TestMethod]
        public void PriceForQuantity_OnBuy1Get50PercentOff_ShouldReturnDiscountedPricing()
        {
            var decorator = new AddOnPercentPromo(_productItem, 1, 50);
            var expectedPriceInCents = 3000 + 1500; // 1 fulprice + 1 halfprice
            Assert.AreEqual(expectedPriceInCents, decorator.GetEffectivePrice(2));
        }

        [TestMethod]
        public void PriceForQuantity_OnAboveEligibleUnits_ShouldStillApplyOnlyEligibleSalePricing()
        {
            var decorator = new AddOnPercentPromo(_productItem, 1, 50); //buy one get 50% off
            var expectedPriceInCents = 9000; // 25% is elgible discount applied on total
            Assert.AreEqual(expectedPriceInCents, decorator.GetEffectivePrice(4));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid salePercent Allowed")]
        public void PriceForQuantity_OnInvalidSalePercent_ShouldThrowException()
        {
            var decorator = new AddOnPercentPromo(_productItem, 1, 101);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid eligibleUnits allowed")]
        public void PriceForQuantity_OnInvalidEligibleUnits_ShouldThrowException()
        {
            var decorator = new AddOnUnitPromo(_productItem, 0, 50);
        }
    }
}