using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Trading;
using Core.Trading.Indicators.Options;

namespace Core.Trading.Indicators
{
	public class Cci : BaseIndicator
	{
		public override string Name => nameof(Cci);

		public override dynamic Get (IEnumerable<ICandle> source, IIndicatorOptions options = null)
		{
			CciOptions config = options != null ? (CciOptions)options.Options : new CciOptions(14);

			double[] cciValues = new double[source.Count()];
			double[] highs = source.Select(x => Convert.ToDouble(x.High)).ToArray();
			double[] lows = source.Select(x => Convert.ToDouble(x.Low)).ToArray();
			double[] closes = source.Select(x => Convert.ToDouble(x.Close)).ToArray();

			TicTacTec.TA.Library.Core.RetCode cci = TicTacTec.TA.Library.Core.Cci(0, source.Count() - 1, highs, lows, closes, config.Period, out int outBegIdx, out int outNbElement, cciValues);

			if (cci == TicTacTec.TA.Library.Core.RetCode.Success)
			{
				return FixIndicatorOrdering(cciValues.ToList(), outBegIdx, outNbElement);
			}

			throw new Exception("Could not calculate CCI");
		}

		public override dynamic Get (IEnumerable<decimal?> candles, IIndicatorOptions options = null)
		{
			throw new NotImplementedException();
		}
	}
}
