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
	public class BollingerAwe : BaseStrategy
	{
		public override string Name { get; } = "Bollinger Awe";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 50;

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();

			List<decimal> closes = candles.Select(x => x.Close).ToList();
			BbandItem bb = candles.Bbands(20);
			List<decimal?> fastMa = candles.Ema(3);
			List<decimal?> hl1 = candles.Select(x => (x.High + x.Low) / 2).ToList().Sma(5);
			List<decimal?> hl2 = candles.Select(x => (x.High + x.Low) / 2).ToList().Sma(34);
			List<int> ao = new List<int>();
			MacdItem macd = candles.Macd();

			for (int i = 0; i < hl1.Count; i++)
			{
				if (i > 0)
				{
					if (hl1[i - 1].HasValue && hl2[i - 1].HasValue && hl1[i].HasValue && hl2[i].HasValue)
					{
						ao.Add(hl1[i].Value - hl2[i].Value >= 0
							  ? hl1[i].Value - hl2[i].Value > hl1[i - 1].Value - hl2[i - 1].Value ? 1 : 2
							  : hl1[i].Value - hl2[i].Value > hl1[i - 1].Value - hl2[i - 1].Value ? -1 : -2);
					}
					else
					{
						ao.Add(0);
					}
				}
				else
				{
					ao.Add(0);
				}
			}

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i < 1)
				{
					result.Add(TradingAdviceCode.HOLD);
				}
				else
				{
					if (closes[i] > bb.MiddleBand[i] && // Closed above the bollinger band
						Math.Abs(ao[i]) == 1 &&
						macd.Macd[i] > macd.Signal[i] &&
						fastMa[i] > bb.MiddleBand[i] &&
						fastMa[i - 1] < bb.MiddleBand[i])
					{
						result.Add(TradingAdviceCode.BUY);
					}
					else if (closes[i] < bb.MiddleBand[i] && // Closed above the bollinger band
						Math.Abs(ao[i]) == 2 &&
						fastMa[i] < bb.MiddleBand[i] &&
						fastMa[i - 1] > bb.MiddleBand[i])
					{
						result.Add(TradingAdviceCode.SELL);
					}
					else
					{
						result.Add(TradingAdviceCode.HOLD);
					}
				}
			}

			return result.LastOrDefault();
		}
	}
}