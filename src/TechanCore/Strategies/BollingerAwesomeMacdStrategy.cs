using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class BollingerAwesomeMacdStrategy : BaseStrategy<BollingerAwesomeMacdStrategyOptions>
	{
		public override string Name { get; } = "Bollinger Awesome MACD Strategy";

		public override int MinNumberOfCandles { get; } = 50;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			BollingerAwesomeMacdStrategyOptions options = GetOptions;
			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal> closes = candles.Select(x => x.Close).ToList();
			var bb = candles.BollingerBands(options.BollingerPeriod, options.BollingerDeviationUp, options.BollingerDeviationDown);
			decimal?[] fastMa = candles.Ema(options.EmaPeriod, CandleVariables.CLOSE).Result;
			decimal?[] hl1 = candles.Select(x => (x.High + x.Low) / 2).ToList().Sma(options.SmaFastPeriod).Result;
			decimal?[] hl2 = candles.Select(x => (x.High + x.Low) / 2).ToList().Sma(options.SmaSlowPeriod).Result;
			List<int> ao = new List<int>();
			var macd = candles.Macd(options.FastPeriod, options.SlowPeriod, options.SignalPeriod);

			for (int i = 0; i < hl1.Length; i++)
			{
				if (i > 0)
				{
					if (hl1[i - 1].HasValue && hl2[i - 1].HasValue && hl1[i].HasValue && hl2[i].HasValue)
					{
						ao.Add(hl1[i] - hl2[i] >= 0
							  ? (hl1[i] - hl2[i] > hl1[i - 1] - hl2[i - 1] ? 1 : 2)
							  : hl1[i] - hl2[i] < hl1[i - 1] - hl2[i - 1] ? -1 : -2);
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
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else
				{
					if (closes[i] > bb.MiddleBand[i] && // Closed above the bollinger band
						Math.Abs(ao[i]) == 1 &&
						macd.Macd[i] > macd.Signal[i] &&
						fastMa[i] > bb.MiddleBand[i] &&
						fastMa[i - 1] < bb.MiddleBand[i])
					{
						result.Add((candles.ElementAt(i), TradingAdvices.BUY));
					}
					else if (closes[i] < bb.MiddleBand[i] && // Closed above the bollinger band
						Math.Abs(ao[i]) == 2 &&
						fastMa[i] < bb.MiddleBand[i] &&
						fastMa[i - 1] > bb.MiddleBand[i])
					{
						result.Add((candles.ElementAt(i), TradingAdvices.SELL));
					}
					else
					{
						result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
					}
				}
			}

			return result;
		}

		public BollingerAwesomeMacdStrategy(BollingerAwesomeMacdStrategyOptions options) : base(options)
		{
		}
	}
}