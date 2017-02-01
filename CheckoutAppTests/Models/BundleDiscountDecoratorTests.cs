using CheckoutApp.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CheckoutAppTests.Models
{
    [TestClass()]
    public class BundleDiscountDecoratorTests
    {
        private readonly ProductItem _testItem = new ProductItem("1", "APPLE", 75);

        [TestMethod()]
        public void PriceForQuantity_OnEligibleBundleSize_ShouldReturnBundleDiscountPricing()
        {
            var decorator = new BundleDiscountDecorator(_testItem, 3, 200);
            var expectedPriceInCents = 200 + 75; //3 apples at 2.00 + 1 at 0.75
            Assert.AreEqual(expectedPriceInCents, decorator.PriceForQuantity(4));
        }

        [TestMethod]
        public void PriceForQuantity_OnUnderBundleSize_ShouldReturnActualPricing()
        {
            var decorator = new BundleDiscountDecorator(_testItem, 3, 200);
            var expectedPriceInCents = 2 * 75; // 2 apple at 0.75 actual price
            Assert.AreEqual(expectedPriceInCents, decorator.PriceForQuantity(2));
        }
    }
}