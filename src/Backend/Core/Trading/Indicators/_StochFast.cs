using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Trading;
using Core.Trading.Indicators.Options;
using Core.Trading.Models;

namespace Core.Trading.Indicators
{
	public class StochFast : BaseIndicator
	{
		public override string Name => nameof(StochFast);

		public override dynamic Get (IEnumerable<ICandle> source, IIndicatorOptions options = null)
		{
			StochFastOptions config = options != null ? (StochFastOptions)options.Options : new StochFastOptions(5, 3, TicTacTec.TA.Library.Core.MAType.Sma);

			double[] kValues = new double[source.Count()];
			double[] dValues = new double[source.Count()];

			double[] highs = source.Select(x => Convert.ToDouble(x.High)).ToArray();
			double[] lows = source.Select(x => Convert.ToDouble(x.Low)).ToArray();
			double[] closes = source.Select(x => Convert.ToDouble(x.Close)).ToArray();

			var tema = TicTacTec.TA.Library.Core.StochF(0, source.Count() - 1, highs, lows, closes, config.FastKPeriod, config.FastDPeriod, config.FastDmaType, out int outBegIdx, out int outNbElement, kValues, dValues);

			if (tema == TicTacTec.TA.Library.Core.RetCode.Success)
			{
				return new StochItem()
				{
					D = FixIndicatorOrdering(dValues.ToList(), outBegIdx, outNbElement),
					K = FixIndicatorOrdering(kValues.ToList(), outBegIdx, outNbElement)
				};
			}

			throw new Exception("Could not calculate STOCH");
		}

		public override dynamic Get (IEnumerable<decimal?> candles, IIndicatorOptions options = null)
		{
			throw new NotImplementedException();
		}
	}
}
