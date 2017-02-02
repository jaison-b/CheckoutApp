using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CheckoutApp.Models;
using CheckoutApp.Repository;
using CsvHelper.Configuration;

namespace CheckoutApp
{
    public class CartFactory : ICartFactory
    {
        private readonly IProductRepository _productRepository;
        private readonly IPromotionRepository _promotionRepository;

        // simple dictionary that acts like a service factory to wrap the orderitem in respective
        // PromotionDecorator for the PromoType
        private IDictionary<PromoType, Func<IOrderItem, PromotionInfo, Promotion>> _promotionDecoratorLookup;

        public CartFactory(IPromotionRepository promotionRepository, IProductRepository productRepository)
        {
            _promotionRepository = promotionRepository;
            _productRepository = productRepository;
            InitPromotionDecoratorLookup();
        }

        public Cart CreateCart(Stream ordersInputFile, DateTime orderDate)
        {
            var orderItems = AppUtils.ParseCsv<InputOrder>(ordersInputFile, typeof(InputOrderMapper))
                .GroupBy(order => order.ProductId)
                .ToDictionary(groupedOrder => ToOrderItem(groupedOrder.Key, orderDate),
                    groupedOrder => groupedOrder.Sum(v => v.Quantity));
            return new Cart(orderItems);
        }

        private IOrderItem ToOrderItem(string productId, DateTime orderDate)
        {
            var productInfo = _productRepository.GetProduct(productId);
            if (productInfo == null)
                throw new ArgumentException("No products exists for Product ID: " + productId);
            return DecoratePromotions(productInfo.ToProductItem(),
                _promotionRepository.GetPromotions(productId, orderDate));
        }

        private IOrderItem DecoratePromotions(IOrderItem orderItem, IReadOnlyList<PromotionInfo> promotions)
        {
            if (promotions == null || promotions.Count == 0)
                return orderItem;
            return promotions.Aggregate(orderItem, ToPromotionDecorator);
        }

        private Promotion ToPromotionDecorator(IOrderItem orderItem, PromotionInfo promotionInfo)
        {
            Func<IOrderItem, PromotionInfo, Promotion> decoratorGenerator;
            if (_promotionDecoratorLookup.TryGetValue(promotionInfo.PromoType, out decoratorGenerator))
            {
                return decoratorGenerator(orderItem, promotionInfo);
            }
            throw new ArgumentException("Unsupported PromoType: " + promotionInfo.PromoType);
        }

        private static int DollarsToCents(double dollarAmount)
        {
            return Convert.ToInt32(dollarAmount * 100);
        }

        private void InitPromotionDecoratorLookup()
        {
            _promotionDecoratorLookup = new Dictionary<PromoType, Func<IOrderItem, PromotionInfo, Promotion>>
            {
                { //Sale Price
                    PromoType.SalePrice, (orderItem, promotionInfo) =>
                        new SalePrice(orderItem, promotionInfo.EligibleQuantity, DollarsToCents(promotionInfo.PromoAmount))
                },
                { //Sale Percent
                    PromoType.SalePercent, (orderItem, promotionInfo) =>
                        new SalePercent(orderItem, promotionInfo.EligibleQuantity, Convert.ToInt32(promotionInfo.PromoAmount))
                },
                { //Bundle Discount
                    PromoType.BundleDiscount, (orderItem, promotionInfo) =>
                        new BundleDiscount(orderItem, promotionInfo.EligibleQuantity, DollarsToCents(promotionInfo.PromoAmount))
                },
                { //AddOn Unit
                    PromoType.AddOnUnit, (orderItem, promotionInfo) =>
                    {
                        var freeUnits = Math.Floor(promotionInfo.PromoAmount); //fractional free units not allowed
                        return new AddOnUnitPromo(orderItem, promotionInfo.EligibleQuantity, Convert.ToInt32(freeUnits));
                    }
                },
                { //AddOn Percent
                    PromoType.AddOnPercent, (orderItem, promotionInfo) =>
                        new AddOnPercentPromo(orderItem, promotionInfo.EligibleQuantity, Convert.ToInt32(promotionInfo.PromoAmount))
                }
            };
        }
    }

    internal sealed class InputOrder
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }

    internal sealed class InputOrderMapper : CsvClassMap<InputOrder>
    {
        public InputOrderMapper()
        {
            Map(m => m.ProductId).Name("PRODUCT_ID");
            Map(m => m.Quantity).ConvertUsing(row => Math.Ceiling(decimal.Parse(row.GetField("UNITS")))).Default(0);
        }
    }
}