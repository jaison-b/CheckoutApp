using System;
using CheckoutApp.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CheckoutAppTests.Models
{
    [TestClass()]
    public class AddOnUnitPromoDecoratorTests
    {
        [TestMethod()]
        public void PriceForQuantity_OnBuyOneGetOneOffer_ShouldReturnExpectedPromoPricing()
        {
            var productItem = new ProductItem("1", "Book", 3000);
            var decorator = new AddOnUnitPromo(productItem, 1, 1);
            var expectedFinalPrice = 3000; //Get 2 books for price of 1
            Assert.AreEqual(expectedFinalPrice, decorator.GetPrice(2));
        }

        [TestMethod]
        public void PriceForQuantity_OnBuy3GetOneOffer_ShouldReturnExpectedPromoPricing()
        {
            var productItem = new ProductItem("1", "T-SHIRT", 2000);
            var decorator = new AddOnUnitPromo(productItem, 3, 1);
            var expectedFinalPrice = 6000; //Get 4 t-shirts for price of 1
            Assert.AreEqual(expectedFinalPrice, decorator.GetPrice(4));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalide EligibleUnits Allowed")]
        public void PriceForQuantity_OnInvalidEligibleUnits_ShouldThrowException()
        {
            var productItem = new ProductItem("1", "T-SHIRT", 2000);
            var decorator = new AddOnUnitPromo(productItem, 0, 1);
        }
    }
}