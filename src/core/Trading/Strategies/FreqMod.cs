using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Extensions;
using core.TypeCodes;

namespace core.Trading.Strategies
{
	public class FreqMod : BaseStrategy
	{
		public override string Name { get; } = "Freq Modded";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 100;

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();

			List<decimal?> sma = candles.Sma(100);
			List<decimal> closes = candles.Select(x => x.Close).ToList();
			List<decimal?> adx = candles.Adx();
			List<decimal?> tema = candles.Tema(4);
			List<decimal?> mfi = candles.Mfi(14);
			List<decimal?> cci = candles.Cci(5);
			List<decimal?> rsi = candles.Rsi();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (closes[i] < sma[i] && cci[i] < -100 && adx[i] > 20 && mfi[i] < 30 && rsi[i] < 25)
				{
					result.Add(TradingAdviceCode.BUY);
				}
				else if (cci[i] > 100 && mfi[i] > 80 && rsi[i] > 70)
				{
					result.Add(TradingAdviceCode.SELL);
				}
				else
				{
					result.Add(TradingAdviceCode.HOLD);
				}
			}

			return result.LastOrDefault();
		}
	}
}