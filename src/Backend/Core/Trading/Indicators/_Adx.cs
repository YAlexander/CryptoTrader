using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Trading;
using Core.Trading.Indicators.Options;

namespace Core.Trading.Indicators
{
	public class Adx : BaseIndicator
	{
		public override string Name => nameof(Adx);

		public override dynamic Get (IEnumerable<ICandle> source, IIndicatorOptions options = null)
		{
			//AdxOptions config = options != null ? (AdxOptions)options.Options : new AdxOptions(30);

			double[] adxValues = new double[source.Count()];
			double[] highs = source.Select(x => Convert.ToDouble(x.High)).ToArray();
			double[] lows = source.Select(x => Convert.ToDouble(x.Low)).ToArray();
			double[] closes = source.Select(x => Convert.ToDouble(x.Close)).ToArray();

			TicTacTec.TA.Library.Core.RetCode adx = TicTacTec.TA.Library.Core.Adx(0, source.Count() - 1, highs, lows, closes, config.Period, out int outBegIdx, out int outNbElement, adxValues);

			if (adx == TicTacTec.TA.Library.Core.RetCode.Success)
			{
				return FixIndicatorOrdering(adxValues.ToList(), outBegIdx, outNbElement);
			}

			throw new Exception("Could not calculate EMA!");
		}

		public override dynamic Get (IEnumerable<decimal?> candles, IIndicatorOptions options = null)
		{
			throw new NotImplementedException();
		}
	}
}
