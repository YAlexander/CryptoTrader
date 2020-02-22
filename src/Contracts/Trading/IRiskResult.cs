using System;

namespace Contracts.Trading
{
    public interface IRiskResult
    {
        decimal? StopLosePrice { get; set; }
        
        decimal? TakeProfitPrice { get; set; }
        
        decimal MaxAmount { get; set; }
        
        DateTime? DisableTradingTill { get; set; }
    }
}