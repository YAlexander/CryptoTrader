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
	public class ThreeMAgos : BaseStrategy
	{
		public override string Name { get; } = "Three MAgos";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 15;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			ThreeMAgosPreset preset = null;
			if (!string.IsNullOrWhiteSpace(Preset))
			{
				preset = JsonConvert.DeserializeObject<ThreeMAgosPreset>(Preset);
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();

			List<decimal?> sma = candles.Sma(preset?.Sma ?? 15);
			List<decimal?> ema = candles.Ema(preset?.Ema ?? 15);
			List<decimal?> wma = candles.Wma(preset?.Wma ?? 15);

			List<decimal> closes = candles.Select(x => x.Close).ToList();

			List<string> bars = new List<string>();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (closes[i] > sma[i] && closes[i] > ema[i] && closes[i] > wma[i])
				{
					bars.Add("green");
				}
				else if (closes[i] > sma[i] || closes[i] > ema[i] || closes[i] > wma[i])
				{
					bars.Add("blue");
				}
				else
				{
					bars.Add("red");
				}
			}

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i < 1)
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
				}
				else if (bars[i] == "blue" && bars[i - 1] == "red")
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
				}
				else if (bars[i] == "blue" && bars[i - 1] == "green")
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.SELL));
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

