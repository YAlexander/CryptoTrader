using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Trading;
using Core.Trading.Indicators.Options;

namespace Core.Trading.Indicators
{
	public class Atr : BaseIndicator
	{
		public override string Name => nameof(Atr);

		public override dynamic Get (IEnumerable<ICandle> source, IIndicatorOptions options = null)
		{
			AtrOptions config = options != null ? (AtrOptions)options.Options : new AtrOptions(20);

			double[] atrValues = new double[source.Count()];

			double[] highs = source.Select(x => Convert.ToDouble(x.High)).ToArray();
			double[] lows = source.Select(x => Convert.ToDouble(x.Low)).ToArray();
			double[] closes = source.Select(x => Convert.ToDouble(x.Close)).ToArray();

			TicTacTec.TA.Library.Core.RetCode atr = TicTacTec.TA.Library.Core.Atr(0, source.Count() - 1, highs, lows, closes, config.Period, out int outBegIdx, out int outNbElement, atrValues);

			if (atr == TicTacTec.TA.Library.Core.RetCode.Success)
			{
				return FixIndicatorOrdering(atrValues.ToList(), outBegIdx, outNbElement);
			}

			throw new Exception("Could not calculate ATR");
		}

		public override dynamic Get (IEnumerable<decimal?> candles, IIndicatorOptions options = null)
		{
			throw new NotImplementedException();
		}
	}
}
