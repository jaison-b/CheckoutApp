using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CheckoutApp.Repository;
using CsvHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace CheckoutAppTests.Repository
{
    [TestClass]
    public class PromotionRepositoryTests
    {
        private const string TestPromotionsFile = "TestData/test_promotions.txt";

        private PromotionRepository _promotionRepository;

        [TestInitialize]
        public void Setup()
        {
            _promotionRepository = new PromotionRepository(File.OpenRead(TestPromotionsFile));
        }


        [TestMethod]
        [ExpectedException(typeof(CsvMissingFieldException), "Invalid promotion data was allowed")]
        public void ParsePromotionsInputStream_OnInvalidFile_ShouldThrowException()
        {
            _promotionRepository = new PromotionRepository(File.OpenRead("TestData/test_invalid_promotions.txt"));
        }

        [TestMethod]
        public void ParsePromotionsInputStream_OnNullStartAndEndDateCsvData_ShouldParseAsDefaultDates()
        {
            var promotion = _promotionRepository.GetPromotions("555", DateTime.Now);
            Assert.AreEqual(DateTime.MinValue, promotion.First().StartDate);
            Assert.AreEqual(DateTime.MaxValue, promotion.First().EndDate);
        }

        [TestMethod]
        public void GetPromotions_OnAvailablePromotionsOnProduct_ShouldReturnExpectedPromotions()
        {
            var expected = new PromotionInfo
            {
                ProductId = "111",
                PromoType = PromoType.SalePrice,
                StartDate = ParseDateTime("2017-01-30"),
                EndDate = ParseDateTime("2017-02-15"),
                EligibleQuantity = 1,
                PromoAmount = 0.5
            };
            var results = _promotionRepository.GetPromotions("111", ParseDateTime("2017-02-02"));
            Assert.IsTrue(results.Count == 1);
            Assert.AreEqual(expected, results.First());
        }

        [TestMethod]
        public void GetPromotions_ForExpiredPromotions_ShouldReturnEmptyList()
        {
            var results = _promotionRepository.GetPromotions("111", ParseDateTime("2017-01-01"));
            Assert.IsTrue(results.Count == 0, "Empty result was expected");
        }

        [TestMethod]
        public void GetPromotions_ForFutureDataPromotions_ShouldReturnEmptyListWhenRequestDateIsBeforeStart()
        {
            var results = _promotionRepository.GetPromotions("333", ParseDateTime("2017-02-02"));
            Assert.IsTrue(results.Count == 0, "Empty result was expected");
        }

        [TestMethod]
        public void GetPromotions_ForNoPromotionsOnProduct_ShouldReturnEmptyPromotions()
        {
            var results = _promotionRepository.GetPromotions("SOME_NON_EXISTENT_PRODUCT_ID", DateTime.Now);
            Assert.IsTrue(results.Count == 0);
        }

        private static DateTime ParseDateTime(string dateTimeStr)
        {
            return DateTime.Parse(dateTimeStr, null, DateTimeStyles.RoundtripKind);
        }
    }
}