using System;
using Contracts.Enums;

namespace Contracts.Trading
{
    public interface IRiskResult
    {
        OrderType Type { get; set; }
        
        FillPolitics FillPolitic { get; set; }
        
        decimal? Price { get; set; }
        
        decimal? StopLosePrice { get; set; }
        
        decimal? TakeProfitPrice { get; set; }
        
        decimal MaxAmount { get; set; }
        
        DateTime? DisableTradingTill { get; set; }
    }
}