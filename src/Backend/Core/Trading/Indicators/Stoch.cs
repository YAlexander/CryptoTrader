using Core.Trading.Indicators.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Trading;
using Core.Trading.Models;

namespace Core.Trading.Indicators
{
	public class Stoch : BaseIndicator
	{
		public override string Name => nameof(Stoch);

		public override dynamic Get (IEnumerable<ICandle> source, IIndicatorOptions options = null)
		{
			StochOptions config = options != null ? (StochOptions)options.Options : new StochOptions(5, 2, TicTacTec.TA.Library.Core.MAType.Sma, 3, TicTacTec.TA.Library.Core.MAType.Sma);

			double[] kValues = new double[source.Count()];
			double[] dValues = new double[source.Count()];

			double[] highs = source.Select(x => Convert.ToDouble(x.High)).ToArray();
			double[] lows = source.Select(x => Convert.ToDouble(x.Low)).ToArray();
			double[] closes = source.Select(x => Convert.ToDouble(x.Close)).ToArray();

			TicTacTec.TA.Library.Core.RetCode stoch = TicTacTec.TA.Library.Core.Stoch(0, source.Count() - 1, highs, lows, closes, config.FastKPeriod, config.SlowKPeriod, config.SlowKmaType, config.SlowDPeriod, config.SlowDmaType, out int outBegIdx, out int outNbElement, kValues, dValues);

			if (stoch == TicTacTec.TA.Library.Core.RetCode.Success)
			{
				return new StochItem()
				{
					D = FixIndicatorOrdering(dValues.ToList(), outBegIdx, outNbElement),
					K = FixIndicatorOrdering(kValues.ToList(), outBegIdx, outNbElement)
				};
			}

			throw new Exception("Could not calculate STOCH!");
		}

		public override dynamic Get (IEnumerable<decimal?> candles, IIndicatorOptions options = null)
		{
			throw new NotImplementedException();
		}
	}
}



