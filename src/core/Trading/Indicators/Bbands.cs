using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Trading.Indicators.Options;
using core.Trading.Models;

namespace core.Trading.Indicators
{
	public class Bbands : BaseIndicator
	{
		public override string Name => nameof(Bbands);

		public override dynamic Get (IEnumerable<ICandle> source, IIndicatorOptions options = null)
		{
			BbandsOptions config = options != null ? (BbandsOptions)options.Options : new BbandsOptions(5, 2, 2, TicTacTec.TA.Library.Core.MAType.Sma);

			double[] upperValues = new double[source.Count()];
			double[] middleValues = new double[source.Count()];
			double[] lowerValues = new double[source.Count()];
			double[] closes = source.Select(x => Convert.ToDouble(x.Close)).ToArray();

			var bbands = TicTacTec.TA.Library.Core.Bbands(0, source.Count() - 1, closes, config.Period, config.DevUp, config.DevDown, config.Type, out int outBegIdx, out int outNbElement, upperValues, middleValues, lowerValues);

			if (bbands == TicTacTec.TA.Library.Core.RetCode.Success)
			{
				return new BbandItem()
				{
					UpperBand = FixIndicatorOrdering(upperValues.ToList(), outBegIdx, outNbElement),
					MiddleBand = FixIndicatorOrdering(middleValues.ToList(), outBegIdx, outNbElement),
					LowerBand = FixIndicatorOrdering(lowerValues.ToList(), outBegIdx, outNbElement)
				};
			}

			throw new Exception("Could not calculate Bbands!");
		}

		public override dynamic Get (IEnumerable<decimal?> candles, IIndicatorOptions options = null)
		{
			throw new NotImplementedException();
		}
	}
}
