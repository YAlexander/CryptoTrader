using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Strategies.StrategyOptions;

namespace Core.Trading.Strategies
{
	public class ThreeMAgos : BaseStrategy
	{
		public override string Name { get; } = "Three MAgos";

		public override int MinNumberOfCandles { get; } = 15;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{			
			ThreeMAgosOptions options = Options.GetOptions<ThreeMAgosOptions>();
			
			Validate(candles, options);
			
			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> sma = candles.Sma(options?.Sma ?? 15);
			List<decimal?> ema = candles.Ema(options?.Ema ?? 15);
			List<decimal?> wma = candles.Wma(options?.Wma ?? 15);

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
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else if (bars[i] == "blue" && bars[i - 1] == "red")
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				else if (bars[i] == "blue" && bars[i - 1] == "green")
				{
					result.Add((candles.ElementAt(i), TradingAdvices.SELL));
				}
				else
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
			}

			return result;
		}
	}
}

