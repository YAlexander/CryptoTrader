using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Extensions;
using core.Trading.Models;
using core.Trading.Strategies.Presets;
using core.TypeCodes;
using TinyJson;

namespace core.Trading.Strategies
{
	public class RsiBbands : BaseStrategy
	{
		public override string Name { get; } = "RSI Bbands";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 200;

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			RsiBbandsPreset preset = null;
			if (!string.IsNullOrWhiteSpace(Preset))
			{
				preset = JSONParser.FromJson<RsiBbandsPreset>(Preset);
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();

			List<decimal?> rsi = candles.Rsi(preset?.Rsi ?? 6);
			BbandItem bbands = candles.Bbands(preset?.Bbands ?? 200);
			List<decimal> closes = candles.Select(x => x.Close).ToList();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i < 1)
				{
					result.Add(TradingAdviceCode.HOLD);
				}
				else if (rsi[i - 1] > 50 && rsi[i] <= 50 && closes[i - 1] < bbands.UpperBand[i - 1] && closes[i] > bbands.UpperBand[i])
				{
					result.Add(TradingAdviceCode.SELL);
				}
				else if (rsi[i - 1] < 50 && rsi[i] >= 50 && closes[i - 1] < bbands.LowerBand[i - 1] && closes[i] > bbands.LowerBand[i])
				{
					result.Add(TradingAdviceCode.BUY);
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