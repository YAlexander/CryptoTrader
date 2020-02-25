using System;
using Contracts.Enums;

namespace Contracts.Trading
{
    public interface ITradeInfo
    {
        /// <summary>
        /// Date of the trade
        /// </summary>
        DateTime OpenDate { get; set; }
        DateTime CloseDate { get; set; }
        
        TradeStatus Status { get; set; }
        
        /// <summary>
        /// ExchangeCode
        /// </summary>
        Exchanges Exchange { get; set; }
        
        /// <summary>
        /// Asset code
        /// </summary>
        Assets Asset1 { get; set; }
        
        /// <summary>
        /// Asset code
        /// </summary>
        Assets Asset2 { get; set; }
        
        TradePositions TradePosition { get; set; }
        
        decimal? BidPrice { get; set; }
        decimal? AskPrice { get; set; }
        decimal? Volume { get; set; }
        
        long? StrategyId { get; set; }
        
        long?[] OrderIds { get; set; }
    }
}