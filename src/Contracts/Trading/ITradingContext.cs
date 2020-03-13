using System;
using System.Collections.Generic;
using Contracts.Enums;

namespace Contracts.Trading
{
	public interface ITradingContext
	{
		long DealId { get; set; }
		
		Exchanges Exchange { get; set; }

		(Assets asset1, Assets asset2) TradingPair { get; set; }

		Timeframes TimeFrame { get; set; }
		
		ICandle[] Candles { get; set; }
		
		ITradingStrategy<IStrategyOption> Strategy { get; set; }
		
		OrderType Type { get; set; }
		
		FillPolitics FillPolitic { get; set; }
		
		decimal? Price { get; set; }
		
		decimal? StopLosePrice { get; set; }
		
		decimal? TakeProfitPrice { get; set; }
		
		decimal MaxAmount { get; set; }
		
		DateTime? DisableTradingTill { get; set; }		
		
		bool CreateReverseOrder { get; set; }
		
		//List<IOrder> Orders { get; set; }
		
		List<IAccount> Funds { get; set; }
		
		TradingAdvices TradingAdvice { get; set; }
	}
}