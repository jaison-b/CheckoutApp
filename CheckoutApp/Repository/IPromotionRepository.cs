using System;
using System.Collections.Generic;

namespace CheckoutApp.Repository
{
    public interface IPromotionRepository
    {
        IReadOnlyList<PromotionInfo> GetPromotions(string productId, DateTime requestDate);
    }
}