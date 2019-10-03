using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Extensions;
using core.Trading.Models;
using core.TypeCodes;

namespace core.Trading.Strategies
{
	public class StochRsiMacd : BaseStrategy
	{
		public override string Name { get; } = "STOCH RSI MACD";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 99;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();

			MacdItem macd = candles.Macd(7, 12, 9);
			StochItem stoch = candles.StochRsi(9, CandleVariableCode.CLOSE, 3, 3, TicTacTec.TA.Library.Core.MAType.Sma);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (macd.Macd[i] - macd.Signal[i] < 0 && stoch.K[i] > 70 && stoch.K[i] < stoch.D[i])
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.SELL));
				}
				else if (macd.Macd[i] - macd.Signal[i] > 0 && stoch.K[i] < 30 && stoch.K[i] > stoch.D[i])
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
				}
				else
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
				}
			}

			return result;
		}
	}
}
