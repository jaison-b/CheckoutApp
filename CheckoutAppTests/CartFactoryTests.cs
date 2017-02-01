using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CheckoutApp;
using CheckoutApp.Models;
using CheckoutApp.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CheckoutAppTests
{
    [TestClass()]
    public class CartFactoryTests
    {
        private readonly Mock<IPromotionRepository> _mockPromotionRepository = new Mock<IPromotionRepository>();
        private readonly Mock<IProductRepository> _mockProductRepository = new Mock<IProductRepository>();

        private ICartFactory _cartFactory;

        private DateTime TEST_ORDER_DATE = DateTime.Now;

        [TestInitialize]
        public void setup()
        {
            _cartFactory = new CartFactory(_mockPromotionRepository.Object, _mockProductRepository.Object);
            //Setup Products
            _mockProductRepository.Setup(repo => repo.GetProduct("111"))
                .Returns(new ProductInfo {ProductId = "111", ProductName = "APPLE", UnitPriceInCents = 75});
            _mockProductRepository.Setup(repo => repo.GetProduct("222"))
                .Returns(new ProductInfo {ProductId = "222", ProductName = "ORANGE", UnitPriceInCents = 50});

            //Setup Promotions
            _mockPromotionRepository.Setup(repo => repo.GetPromotions("111", TEST_ORDER_DATE))
                .Returns(new List<PromotionInfo> {GetAppleSalePricePromo()}.AsReadOnly);
        }

        private static PromotionInfo GetAppleSalePricePromo()
        {
            return new PromotionInfo
            {
                ProductId = "111",
                PromoType = PromoType.SalePrice,
                EligibleQuantity = 1,
                StartDate = DateTime.Today.AddDays(-1),
                EndDate = DateTime.Today.AddDays(120),
                PromoAmount = 0.50
            };
        }

        [TestMethod]
        public void CreateCart_ForValidInput_ShouldReturnExpectedPromoDecoratedItems()
        {
            Cart cart = _cartFactory.CreateCart(GetTestOrdersStream(), TEST_ORDER_DATE);
            Assert.IsTrue(cart.GetOrderItems().Count == 2, "Expected 2 Cart Items");
            var appleProductItem = new ProductItem("111", "APPLE", 75);
            var expectedDecoratedItem = new SalePriceDecorator(appleProductItem, 1, 50);
            Assert.IsTrue(
                cart.GetOrderItems()
                    .Any(keyValue => keyValue.Key.Equals(expectedDecoratedItem) && keyValue.Value.Equals(7)),
                "Card Items didn't match expected ProductItem: " + appleProductItem);
        }

        [TestMethod]
        public void CreateCart_ForItemsWithNoPromotion_ShouldReturnActualPricing()
        {
            Cart cart = _cartFactory.CreateCart(GetTestOrdersStream(), TEST_ORDER_DATE);
            var expectedPriceInCents = 2 * 50; //2 organges 50 cents each
            var actualItem = cart.GetOrderItems().Single(item => item.Key.ItemId().Equals("222"));
            Assert.AreEqual(expectedPriceInCents, actualItem.Key.PriceForQuantity(actualItem.Value));
        }

        [TestMethod]
        public void CreateCart_ForItemsWithMultipleEligiblePromotion_ShouldReturnAllAppliedPromotionPricing()
        {
            //Apple have SalePrice and BuyOneGetOne promo as well
            var buyOneGetOnePromo = new PromotionInfo
            {
                ProductId = "111",
                PromoType = PromoType.AddOnUnit,
                StartDate = DateTime.Now,
                EndDate = DateTime.Today.AddDays(120),
                EligibleQuantity = 1,
                PromoAmount = 1
            };
            _mockPromotionRepository.Setup(repo => repo.GetPromotions("111", TEST_ORDER_DATE))
                .Returns(new List<PromotionInfo> {GetAppleSalePricePromo(), buyOneGetOnePromo});

            var expectedPriceInCents = 175; // 7 apples at sale price(0.50) and buy one get one offer on top of that
            Cart cart = _cartFactory.CreateCart(GetTestOrdersStream(), TEST_ORDER_DATE);
            var actualItem = cart.GetOrderItems().Single(item => item.Key.ItemId().Equals("111"));
            Assert.AreEqual(expectedPriceInCents, actualItem.Key.PriceForQuantity(actualItem.Value));
        }

        [TestMethod]
        public void CreateCart_ForExpiredPromos_ShouldReturnActualPricingForItem()
        {
            _mockPromotionRepository.Setup(repo => repo.GetPromotions("111", TEST_ORDER_DATE))
                .Returns(new List<PromotionInfo>()); //No promotions available
            var cart = _cartFactory.CreateCart(GetTestOrdersStream(), TEST_ORDER_DATE);
            var expectedPriceInCents = 7 * 75; // 7 apples at actual price(0.75)
            var actualItem = cart.GetOrderItems().Single(item => item.Key.ItemId().Equals("111"));
            Assert.AreEqual(expectedPriceInCents, actualItem.Key.PriceForQuantity(actualItem.Value));
        }

        private Stream GetTestOrdersStream()
        {
            return new MemoryStream(Encoding.ASCII.GetBytes(@"PRODUCT_ID, UNITS
                                                              111, 5
                                                              222, 2
                                                              111, 1.5"));
        }
    }
}