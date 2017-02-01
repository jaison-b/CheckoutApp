using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CheckoutApp.Models;
using CheckoutApp.Repository;
using CsvHelper.Configuration;
using Fclp.Internals.Extensions;

namespace CheckoutApp
{
    public interface ICartFactory
    {
        Cart CreateCart(Stream ordersInputFile, DateTime orderDate);
    }

    public class CartFactory : ICartFactory
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly IProductRepository _productRepository;

        public CartFactory(IPromotionRepository promotionRepository, IProductRepository productRepository)
        {
            _promotionRepository = promotionRepository;
            _productRepository = productRepository;
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
            {
                throw new ArgumentException("No products exists for Product ID: " + productId);
            }
            return DecoratePromotions(productInfo.ToProductItem(), _promotionRepository.GetPromotions(productId,orderDate));
        }

        private IOrderItem DecoratePromotions(IOrderItem orderItem, IReadOnlyList<PromotionInfo> promotions)
        {
            return promotions.IsNullOrEmpty()
                ? orderItem
                : promotions.Aggregate(orderItem, (current, promotion) => ToPromotionDecorator(current, promotion));
        }

        private PromotionDecorator ToPromotionDecorator(IOrderItem orderItem, PromotionInfo promotionInfo)
        {
            switch (promotionInfo.PromoType)
            {
                case PromoType.SalePrice:
                    return new SalePriceDecorator(orderItem, promotionInfo.EligibleQuantity,
                        DollarsToCents(promotionInfo.PromoAmount));
                case PromoType.SalePercent:
                    return new SalePercentDecorator(orderItem, promotionInfo.EligibleQuantity,
                        Convert.ToInt32(promotionInfo.PromoAmount));
                case PromoType.BundleDiscount:
                    return new BundleDiscountDecorator(orderItem, promotionInfo.EligibleQuantity,
                        DollarsToCents(promotionInfo.PromoAmount));
                case PromoType.AddOnUnit:
                    var freeUnits = Math.Floor(promotionInfo.PromoAmount); //fractional free units not allowed
                    return new AddOnUnitPromoDecorator(orderItem, promotionInfo.EligibleQuantity,
                        Convert.ToInt32(freeUnits));
                case PromoType.AddOnPercent:
                    return new AddOnPercentPromoDecorator(orderItem, promotionInfo.EligibleQuantity,
                        Convert.ToInt32(promotionInfo.PromoAmount));
                    
            }
            throw new ArgumentException("Unsupported PromoType: " + promotionInfo.PromoType);
        }

        private static int DollarsToCents(double dollarAmount)
        {
            return Convert.ToInt32(dollarAmount * 100);
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
}