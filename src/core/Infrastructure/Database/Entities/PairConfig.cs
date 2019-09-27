using System;

namespace core.Infrastructure.Database.Entities
{
	public class PairConfig : BaseEntity
	{
		public int ExchangeCode { get; set; }
		public string AssetOne { get; set; }
		public string AssetTwo { get; set; }
		public string Symbol { get; set; }
		public bool IsEnabled { get; set; }
		public int? StrategyId { get; set; }
		public int? RefreshDelaySeconds { get; set; }
		public float? DefaultStopLossPercent { get; set; }
		public float? DefaultTakeProfitPercent { get; set; }
		public bool IsTestMode { get; set; }
		public float? ExchangeFeeSell { get; set; }
		public float? ExchangeFeeBuy { get; set; }
		public DateTime? TradingLockedTill { get; set; }
		public decimal? MaxOrderAmount { get; set; }
		public decimal? MinOrderAmount { get; set; }

		public int? Timeframe { get; set; }
	}
}
