using System.Collections.Generic;
using Contracts.Enums;

namespace Contracts.Trading
{
	public interface ITradingContext
	{
		Exchanges Exchange { get; set; }

		(Assets asset1, Assets asset2) TradingPair { get; set; }

		Timeframes TimeFrame { get; set; }
		
		ICandle[] Candles { get; set; }

		ITradingStrategy<IStrategyOption> Strategy { get; set; }
		
		IRisk[] Risks { get; set; }
		
		//List<IOrder> Orders { get; set; }
		
		//List<IAccount> Funds { get; set; }
	}
}