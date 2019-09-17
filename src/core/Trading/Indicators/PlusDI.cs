using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using Core.Trading.Indicators.Options;

namespace core.Trading.Indicators
{
	public class PlusDi : BaseIndicator
	{
		public override string Name => nameof(PlusDi);

		public override dynamic Get (IEnumerable<ICandle> source, IIndicatorOptions options = null)
		{
			PlusDiOptions config = options != null ? (PlusDiOptions)options.Options : new PlusDiOptions(14);

			double[] diValues = new double[source.Count()];

			double[] highs = source.Select(x => Convert.ToDouble(x.High)).ToArray();
			double[] lows = source.Select(x => Convert.ToDouble(x.Low)).ToArray();
			double[] closes = source.Select(x => Convert.ToDouble(x.Close)).ToArray();

			TicTacTec.TA.Library.Core.RetCode plusDi = TicTacTec.TA.Library.Core.PlusDI(0, source.Count() - 1, highs, lows, closes, config.Period, out int outBegIdx, out int outNbElement, diValues);

			if (plusDi == TicTacTec.TA.Library.Core.RetCode.Success)
			{
				return FixIndicatorOrdering(diValues.ToList(), outBegIdx, outNbElement);
			}

			throw new Exception("Could not calculate +DI!");
		}

		public override dynamic Get (IEnumerable<decimal?> candles, IIndicatorOptions options = null)
		{
			throw new NotImplementedException();
		}
	}
}
