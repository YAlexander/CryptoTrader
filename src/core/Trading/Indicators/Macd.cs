using core.Abstractions;
using core.Trading.Models;
using Core.Trading.Indicators.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace core.Trading.Indicators
{
	public class Macd : BaseIndicator
	{
		public override string Name => nameof(Macd);

		public override dynamic Get (IEnumerable<ICandle> source, IIndicatorOptions options = null)
		{
			MacdOptions config = options != null ? (MacdOptions)options.Options : new MacdOptions(26, 12, 9);

			double[] macdValues = new double[source.Count()];
			double[] signalValues = new double[source.Count()];
			double[] histValues = new double[source.Count()];
			double[] closes = source.Select(x => Convert.ToDouble(x.Close)).ToArray();

			TicTacTec.TA.Library.Core.RetCode macd = TicTacTec.TA.Library.Core.Macd(0, source.Count() - 1, closes, config.FastPeriod, config.SlowPeriod, config.SignalPeriod, out int outBegIdx, out int outNbElement, macdValues, signalValues, histValues);

			if (macd == TicTacTec.TA.Library.Core.RetCode.Success)
			{
				return new MacdItem()
				{
					Macd = FixIndicatorOrdering(macdValues.ToList(), outBegIdx, outNbElement),
					Signal = FixIndicatorOrdering(signalValues.ToList(), outBegIdx, outNbElement),
					Hist = FixIndicatorOrdering(histValues.ToList(), outBegIdx, outNbElement)
				};
			}

			throw new Exception("Could not calculate MACD!");
		}

		public override dynamic Get (IEnumerable<decimal?> candles, IIndicatorOptions options = null)
		{
			throw new NotImplementedException();
		}
	}
}
