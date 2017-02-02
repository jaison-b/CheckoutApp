using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace CheckoutApp.Repository
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly IList<PromotionInfo> _promotions;

        public PromotionRepository(Stream promotionsInputStream)
        {
            _promotions = AppUtils.ParseCsv<PromotionInfo>(promotionsInputStream, typeof(PromotionInfoMapper));
        }

        public IReadOnlyList<PromotionInfo> GetPromotions(string productId, DateTime requestDate)
        {
            return _promotions.Where(
                    promoInfo => string.Equals(promoInfo.ProductId, productId, StringComparison.OrdinalIgnoreCase)
                                 && requestDate >= promoInfo.StartDate && requestDate <= promoInfo.EndDate)
                .ToList()
                .AsReadOnly();
        }
    }

    internal sealed class PromotionInfoMapper : CsvClassMap<PromotionInfo>
    {
        public PromotionInfoMapper()
        {
            Map(m => m.ProductId).Name("PRODUCT_ID");
            Map(m => m.PromoType).ConvertUsing(row =>
            {
                var value = row.GetField<string>("PROMO_TYPE");
                if (string.IsNullOrEmpty(value))
                    throw new CsvMissingFieldException("Missing PromoType field on row: " + row.Row);
                return (PromoType) Enum.Parse(typeof(PromoType), value, true);
            });
            Map(m => m.StartDate)
                .Name("START_DATE")
                .TypeConverterOption(DateTimeStyles.RoundtripKind)
                .Default(DateTime.MinValue);
            Map(m => m.EndDate)
                .Name("END_DATE")
                .TypeConverterOption(DateTimeStyles.RoundtripKind)
                .Default(DateTime.MaxValue);
            Map(m => m.EligibleQuantity)
                .ConvertUsing(row => Math.Floor(decimal.Parse(row.GetField("ELIGIBLE_QUANTITY"))));
            Map(m => m.PromoAmount).Name("PROMO_AMOUNT").TypeConverterOption(NumberStyles.Float);
        }
    }
}