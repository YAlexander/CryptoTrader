using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using Core.Trading.Indicators.Options;

namespace core.Trading.Indicators
{
	public class MinusDi : BaseIndicator
	{
		public override string Name => nameof(MinusDi);

		public override dynamic Get (IEnumerable<ICandle> source, IIndicatorOptions options = null)
		{
			MinusDiOptions config = options != null ? (MinusDiOptions)options.Options : new MinusDiOptions(14);

			double[] diValues = new double[source.Count()];
			double[] highs = source.Select(x => Convert.ToDouble(x.High)).ToArray();
			double[] lows = source.Select(x => Convert.ToDouble(x.Low)).ToArray();
			double[] closes = source.Select(x => Convert.ToDouble(x.Close)).ToArray();

			TicTacTec.TA.Library.Core.RetCode minusDi = TicTacTec.TA.Library.Core.MinusDI(0, source.Count() - 1, highs, lows, closes, config.Period, out int outBegIdx, out int outNbElement, diValues);

			if (minusDi == TicTacTec.TA.Library.Core.RetCode.Success)
			{
				return FixIndicatorOrdering(diValues.ToList(), outBegIdx, outNbElement);
			}

			throw new Exception("Could not calculate EMA");
		}

		public override dynamic Get (IEnumerable<decimal?> candles, IIndicatorOptions options = null)
		{
			throw new NotImplementedException();
		}
	}
}
