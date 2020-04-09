using System;
using System.Collections.Generic;
using Abstractions.Entities;
using Abstractions.Enums;
using TechanCore;
using TechanCore.Enums;

namespace Abstractions
{
	public interface ITradingContext
	{
		Guid? DealId { get; set; }
		
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

		// TODO: Add Deal and Orders entity
		
		List<IBalance> Funds { get; set; }
		
		TradingAdvices TradingAdvice { get; set; }
	}
}