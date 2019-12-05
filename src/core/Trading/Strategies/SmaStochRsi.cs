using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Extensions;
using core.Trading.Models;
using core.Trading.Strategies.Presets;
using core.TypeCodes;

namespace core.Trading.Strategies
{
	public class SmaStochRsi : BaseStrategy
	{
		public override string Name { get; } = "SMA Stoch RSI";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 150;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			SmaStochRsiPreset preset = null;
			if (!string.IsNullOrWhiteSpace(Preset))
			{
				preset = JsonSerializer.Deserialize<SmaStochRsiPreset>(Preset);
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();
			List<decimal> price = candles.Select(x => x.Close).ToList();

			StochItem stoch = candles.Stoch(preset?.Stoch ?? 8);
			List<decimal?> sma = candles.Sma(preset?.Sma ?? 150);
			List<decimal?> rsi = candles.Rsi(preset?.Rsi ?? 3);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i < 1)
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
				}
				else
				{
					if (price[i] > sma[i] && stoch.K[i] > 70 && rsi[i] < 20 && stoch.K[i] > stoch.D[i])
					{
						result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
					}
					else if (price[i] < sma[i] && stoch.K[i] > 70 && rsi[i] > 80 && stoch.K[i] < stoch.D[i])
					{
						result.Add((candles.ElementAt(i), TradingAdviceCode.SELL));
					}
					else
					{
						result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
					}
				}
			}

			return result;
		}
	}
}

