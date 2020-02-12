using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Strategies.StrategyOptions;

namespace Core.Trading.Strategies
{
	public class CciEma : BaseStrategy
	{
		public override string Name { get; } = "CCI EMA";

		public override int MinNumberOfCandles { get; } = 30;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			CciEmaOptions options = Options.GetOptions<CciEmaOptions>();

			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> cci = candles.Cci(options?.Cci ?? 30);
			List<decimal?> emaFast = candles.Ema(options?.EmaFast ?? 8);
			List<decimal?> emaSlow = candles.Ema(options?.EmaSlow ?? 28);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else if (cci[i] < -100 && emaFast[i] > emaSlow[i] && emaFast[i - 1] < emaSlow[i])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				else if (cci[i] > 100 && emaFast[i] < emaSlow[i] && emaFast[i - 1] > emaSlow[i])
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
