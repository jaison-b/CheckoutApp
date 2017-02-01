using System;
using System.IO;
using CheckoutApp.Repository;
using CsvHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CheckoutAppTests.Repository
{
    [TestClass]
    public class ProductRepositoryTests
    {
        private const string TestProductsFile = "TestData/test_products.txt";

        private ProductRepository _productRepository;

        [TestInitialize]
        public void Setup()
        {
            _productRepository = new ProductRepository(File.OpenRead(TestProductsFile));
        }

        [TestMethod]
        [ExpectedException(typeof(CsvMissingFieldException), "Missing Unit Price file was allowed")]
        public void ParseProductsInputStream_OnMissingUnitPrice_ShouldThrowException()
        {
            _productRepository = new ProductRepository(File.OpenRead("TestData/test_missing_price_products.txt"));
        }

        [TestMethod]
        public void GetProduct_OnValidProductId_ShouldReturnExpectedProductInfo()
        {
            var productInfo = _productRepository.GetProduct("111");
            var expected = new ProductInfo {ProductId = "111", ProductName = "APPLE", UnitPriceInCents = 75};
            Assert.AreEqual(expected, productInfo);
        }

        [TestMethod]
        public void GetProduct_OnInvalidProductId_ShouldReturnNull()
        {
            var productInfo = _productRepository.GetProduct("SOME_NON_EXISTENT_ID");
            Assert.IsNull(productInfo);
        }
    }
}