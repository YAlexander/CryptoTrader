using System;
using System.Collections.Generic;
using Abstractions;
using Abstractions.Entities;
using Abstractions.Enums;
using TechanCore;
using TechanCore.Enums;

namespace Core.BusinessLogic
{
	public class TradingContext : ITradingContext
	{
		public TradingMode Mode { get; set; }
		public IDeal Deal { get; set; }
		public Exchanges Exchange { get; set; }
		public (Assets asset1, Assets asset2) TradingPair { get; set; }
		public Timeframes TimeFrame { get; set; }
		public ICandle[] Candles { get; set; }
		public ITradingStrategy<IStrategyOption> Strategy { get; set; }
		
		public OrderType Type { get; set; }
		public FillPolitics FillPolitic { get; set; }
		
		public decimal? Price { get; set; }
		public decimal? StopLosePrice { get; set; }
		public decimal? TakeProfitPrice { get; set; }
		public decimal MaxAmount { get; set; }
		public DateTime? DisableTradingTill { get; set; }
		public bool CreateReverseOrder { get; set; }
		public IBalance[] Funds { get; set; }
		public TradingAdvices TradingAdvice { get; set; }
		public decimal? LastTrade { get; set; }
		
		public List<IOrder> Orders { get; }
		
		// Max daily loss
		public decimal? DailyRiskLimit { get; set; }
	}
}