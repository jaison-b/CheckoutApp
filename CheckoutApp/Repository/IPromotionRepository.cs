using CheckoutApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace CheckoutApp.Repository
{
    public interface IPromotionRepository
    {
        IReadOnlyList<PromotionInfo> GetPromotions(string productId);
    }

    public class PromotionRepository : IPromotionRepository
    {
        private readonly IList<PromotionInfo> _promotions;

        public PromotionRepository(Stream promotionsInputStream)
        {
            _promotions = ParsePromotionsInputStream(promotionsInputStream);
        }

        private static IList<PromotionInfo> ParsePromotionsInputStream(Stream promotionsInputStream)
        {
            var csv = new CsvReader(new StreamReader(promotionsInputStream));
            csv.Configuration.TrimFields = true;
            csv.Configuration.TrimHeaders = true;
            csv.Configuration.RegisterClassMap<PromotionInfoMapper>();
            csv.Configuration.WillThrowOnMissingField = false;
            return csv.GetRecords<PromotionInfo>().ToList();
        }

        public IReadOnlyList<PromotionInfo> GetPromotions(string productId)
        {
            return _promotions.Where(
                    promoInfo => string.Equals(promoInfo.ProductId, productId, StringComparison.OrdinalIgnoreCase))
                .ToList()
                .AsReadOnly();
        }
    }

    internal sealed class PromotionInfoMapper : CsvClassMap<PromotionInfo>
    {
        public PromotionInfoMapper()
        {
            Map(m => m.ProductId).Name("PRODUCT_ID");
            Map(m => m.PromoType).ConvertUsing(row => ParseEnum<PromoType>(row.GetField<string>("PROMO_TYPE"), "Missing PromoType field on row: " + row.Row));
            Map(m => m.StartDate).Name("START_DATE").TypeConverterOption(DateTimeStyles.RoundtripKind).Default(DateTime.MinValue);
            Map(m => m.EndDate).Name("END_DATE").TypeConverterOption(DateTimeStyles.RoundtripKind).Default(DateTime.MaxValue);
            Map(m => m.EligibleQuantity).Name("ELIGIBLE_QUANTITY").TypeConverterOption(NumberStyles.Integer);
            Map(m => m.PromoAmount).Name("PROMO_AMOUNT").TypeConverterOption(NumberStyles.Float);
            Map(m => m.PromoUnit).ConvertUsing(row => ParseEnum<PromoUnit>(row.GetField<string>("PROMO_UNIT"), "Missing PromoUnit field on row: " + row.Row));
        }

        private static T ParseEnum<T>(string value, string messageOnValueMissing)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new CsvMissingFieldException(messageOnValueMissing);
            }
            return (T) Enum.Parse(typeof(T), value, true);
        }
    }
}