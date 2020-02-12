using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Contracts.TypeCodes;
using Core.Trading.Extensions;
using Core.Trading.Strategies.StrategyOptions;
using Core.TypeCodes;

namespace Core.Trading.Strategies
{
	public class BreakoutMa : BaseStrategy
	{
		public override string Name { get; } = "Breakout MA";

		public override int MinNumberOfCandles { get; } = 35;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			ICandleVariableCode defaultVariableCode = CandleVariableCode.Low;

			BreakoutMaOptions options = Options.GetOptions<BreakoutMaOptions>();

			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> sma20 = candles.Sma(options?.Sma ?? 20, options != null ? CandleVariableCode.Create(options.SmaCandleVariableCode) : defaultVariableCode);
			List<decimal?> ema34 = candles.Ema(options?.Ema ?? 34);
			List<decimal?> adx = candles.Adx(options?.Adx ?? 13);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else if (ema34[i - 1] > sma20[i - 1] && ema34[i] < sma20[i] && adx[i] > 25)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				else if (ema34[i] > sma20[i] && ema34[i - 1] < sma20[i - 1] && adx[i] > 25)
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
