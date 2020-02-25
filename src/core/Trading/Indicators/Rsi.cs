using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using Core.Trading.Indicators.Options;

namespace core.Trading.Indicators
{
	public class Rsi : BaseIndicator
	{
		public override string Name => nameof(Rsi);

		public override dynamic Get (IEnumerable<ICandle> source, IIndicatorOptions options = null)
		{
			IEnumerable<decimal?> closes = source.Select(x => (decimal?)x.Close);
			return Get(closes, options);
		}

		public override dynamic Get (IEnumerable<decimal?> source, IIndicatorOptions options = null)
		{
			RsiOptions config = options != null ? (RsiOptions)options.Options : new RsiOptions(14);

			double[] rsiValues = new double[source.Count()];

			double[] sourceFix = source.Select(x => Convert.ToDouble(x)).ToArray();

			TicTacTec.TA.Library.Core.RetCode rsi = TicTacTec.TA.Library.Core.Rsi(0, source.Count() - 1, sourceFix, config.Period, out int outBegIdx, out int outNbElement, rsiValues);

			if (rsi == TicTacTec.TA.Library.Core.RetCode.Success)
			{
				return FixIndicatorOrdering(rsiValues.ToList(), outBegIdx, outNbElement);
			}

			throw new Exception("Could not calculate RSI");
		}
	}
}
