using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;

namespace Core.Trading.Strategies
{
	public class DerivativeOscillator : BaseStrategy
	{
		public override string Name { get; } = "Derivative Oscillator";

		public override int MinNumberOfCandles { get; } = 20;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();
			List<decimal?> derivativeOsc = candles.DerivativeOscillator();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else if (derivativeOsc[i - 1] < 0 && derivativeOsc[i] > 0)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				else if (derivativeOsc[i] >= 0 && derivativeOsc[i] <= derivativeOsc[i - 1])
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