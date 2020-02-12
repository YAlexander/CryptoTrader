using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Trading;
using Core.Trading.Extensions;

namespace Core.Trading.Indicators
{
	public class DerivativeOscillator : BaseIndicator
	{
		public override string Name => nameof(DerivativeOscillator);

		public override dynamic Get (IEnumerable<ICandle> source, IIndicatorOptions options = null)
		{
			List<decimal?> rsi = source.Rsi(14);

			IEnumerable<decimal?> ema1Source = rsi.Where(x => x.HasValue).Select(x => (decimal?)x.Value);
			List<decimal?> ema1 = ema1Source.Ema(5);

			IEnumerable<decimal?> ema2Source = ema1.Where(x => x.HasValue).Select(x => (decimal?)x.Value);
			List<decimal?> ema2 = ema2Source.Ema(3);

			IEnumerable<decimal?> smaSource = ema2.Where(x => x.HasValue).Select(x => (decimal?)x.Value);
			List<decimal?> sma = smaSource.Sma(9);

			for (int i = sma.Count; i < source.Count(); i++)
			{
				sma.Insert(0, null);
			}

			for (int i = ema2.Count; i < source.Count(); i++)
			{
				ema2.Insert(0, null);
			}

			List<decimal?> derivativeOsc = new List<decimal?>();

			for (int i = 0; i < sma.Count; i++)
			{
				if (sma[i] == null || ema2[i] == null)
				{
					derivativeOsc.Add(null);
				}
				else
				{
					derivativeOsc.Add(ema2[i] - sma[i]);
				}
			}

			return derivativeOsc;
		}

		public override dynamic Get (IEnumerable<decimal?> candles, IIndicatorOptions options = null)
		{
			throw new NotImplementedException();
		}
	}
}



