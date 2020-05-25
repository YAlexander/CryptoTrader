using System.Text.Json;
using Abstractions;
using Abstractions.Entities;
using Common;
using Persistence.Entities;

namespace Persistence.Helpers
{
    public static class KeyHelper
    {
        public static GrainKeyExtension ToExtendedKey (this string key)
        {
            return JsonSerializer.Deserialize<GrainKeyExtension>(key);
        }

        public static GrainKeyExtension ToExtendedKey(this CandleEntity candle)
        {
            return new GrainKeyExtension
            {
                Exchange = candle.Exchange,
                Asset1 = candle.Asset1,
                Asset2 = candle.Asset2,
                Time = candle.Time
            };
        }
        
        public static GrainKeyExtension ToExtendedKey(this OrderEntity order)
        {
            return new GrainKeyExtension
            {
                Exchange = order.Exchange,
                Asset1 = order.Asset1,
                Asset2 = order.Asset2,
                Id = order.Id
            };
        }
        
        public static GrainKeyExtension ToExtendedKey(this IOrder order)
        {
            return new GrainKeyExtension
            {
                Exchange = order.Exchange,
                Asset1 = order.Asset1,
                Asset2 = order.Asset2,
            };
        }
        
        public static GrainKeyExtension ToExtendedKey(this ITradingContext context)
        {
            return new GrainKeyExtension
            {
                Exchange = context.Exchange,
                Asset1 = context.TradingPair.asset1,
                Asset2 = context.TradingPair.asset2,
            };
        }
    }
}