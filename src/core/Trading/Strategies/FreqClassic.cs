using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Extensions;
using core.Trading.Extensions;
using core.Trading.Models;
using core.TypeCodes;

namespace core.Trading.Strategies
{
	public class FreqClassic : BaseStrategy
	{
		public override string Name { get; } = "Freq Classic";

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
			List<decimal> closes = candles.Close();
			List<decimal?> adx = candles.Adx();
			List<decimal?> tema = candles.Tema(4);
			List<decimal?> mfi = candles.Mfi(14);

			List<decimal?> cci = candles.Cci(5);
			StochItem stoch = candles.StochFast();
			List<decimal?> middleBands = candles.Bbands().MiddleBand;

			List<decimal?> fishers = candles.Fisher();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (closes[i] < sma[i] && cci[i] < -100 && fishers[i] < 0 && adx[i] > 20 && mfi[i] < 30 && tema[i] <= middleBands[i])
				{
					result.Add(TradingAdviceCode.BUY);
				}
				else if (fishers[i] == 1)
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