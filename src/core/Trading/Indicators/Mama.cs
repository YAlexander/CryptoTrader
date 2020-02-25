using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Trading.Models;
using Core.Trading.Indicators.Options;

namespace core.Trading.Indicators
{
	public class Mama : BaseIndicator
	{
		public override string Name => nameof(Mama);

		public override dynamic Get (IEnumerable<ICandle> source, IIndicatorOptions options = null)
		{
			MamaOptions config = options != null ? (MamaOptions)options.Options : new MamaOptions(0, 0);

			double[] mamaValues = new double[source.Count()];
			double[] famaValues = new double[source.Count()];
			double[] closes = source.Select(x => Convert.ToDouble(x.Close)).ToArray();

			var mfi = TicTacTec.TA.Library.Core.Mama(0, source.Count() - 1, closes, config.FastLimit, config.SlowLimit, out int outBegIdx, out int outNbElement, mamaValues, famaValues);

			if (mfi == TicTacTec.TA.Library.Core.RetCode.Success)
			{
				return new MamaItem
				{
					Mama = FixIndicatorOrdering(mamaValues.ToList(), outBegIdx, outNbElement),
					Fama = FixIndicatorOrdering(famaValues.ToList(), outBegIdx, outNbElement)
				};
			}

			throw new Exception("Could not calculate MAMA");
		}

		public override dynamic Get (IEnumerable<decimal?> candles, IIndicatorOptions options = null)
		{
			throw new NotImplementedException();
		}
	}
}
