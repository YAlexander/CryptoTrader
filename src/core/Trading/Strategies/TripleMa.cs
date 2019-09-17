using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Extensions;
using core.Trading.Strategies.Presets;
using core.TypeCodes;
using Newtonsoft.Json;

namespace core.Trading.Strategies
{
	public class TripleMa : BaseStrategy
	{
		public override string Name { get; } = "Triple MA";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 50;

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			TripleMaPreset preset = null;
			if (!string.IsNullOrWhiteSpace(Preset))
			{
				preset = JsonConvert.DeserializeObject<TripleMaPreset>(Preset);
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();

			List<decimal?> sma1 = candles.Sma(preset?.Sma1 ?? 20);
			List<decimal?> sma2 = candles.Sma(preset?.Sma2 ?? 50);
			List<decimal?> ema = candles.Ema(preset?.Ema ?? 11);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add(TradingAdviceCode.HOLD);
				}
				else if (ema[i] > sma2[i] && ema[i - 1] < sma2[i - 1])
				{
					result.Add(TradingAdviceCode.BUY); // A cross of the EMA and long SMA is a buy signal.
				}
				else if (ema[i] < sma2[i] && ema[i - 1] > sma2[i - 1] || ema[i] < sma1[i] && ema[i - 1] > sma1[i - 1])
				{
					result.Add(TradingAdviceCode.SELL); // As soon as our EMA crosses below an SMA its a sell signal.
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
