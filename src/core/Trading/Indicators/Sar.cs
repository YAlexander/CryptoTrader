using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using Core.Trading.Indicators.Options;

namespace core.Trading.Indicators
{
	public class Sar : BaseIndicator
	{
		public override string Name => nameof(Sar);

		public override dynamic Get (IEnumerable<ICandle> source, IIndicatorOptions options = null)
		{
			SarOptions config = options != null ? (SarOptions)options.Options : new SarOptions(0.02, 02);

			double[] sarValues = new double[source.Count()];
			double[] highs = source.Select(x => Convert.ToDouble(x.High)).ToArray();
			double[] lows = source.Select(x => Convert.ToDouble(x.Low)).ToArray();

			TicTacTec.TA.Library.Core.RetCode sar = TicTacTec.TA.Library.Core.Sar(0, source.Count() - 1, highs, lows, 0.02, 0.2, out int outBegIdx, out int outNbElement, sarValues);

			if (sar == TicTacTec.TA.Library.Core.RetCode.Success)
			{
				// party
				return FixIndicatorOrdering(sarValues.ToList(), outBegIdx, outNbElement);
			}

			throw new Exception("Could not calculate SAR");
		}

		public override dynamic Get (IEnumerable<decimal?> candles, IIndicatorOptions options = null)
		{
			throw new NotImplementedException();
		}
	}
}
