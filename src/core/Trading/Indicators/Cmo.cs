using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using Core.Trading.Indicators.Options;

namespace core.Trading.Indicators
{
	public class Cmo : BaseIndicator
	{
		public override string Name => nameof(Cmo);

		public override dynamic Get (IEnumerable<ICandle> source, IIndicatorOptions options = null)
		{
			CmoOptions config = options != null ? (CmoOptions)options.Options : new CmoOptions(14);

			double[] cmoValues = new double[source.Count()];
			double[] valuesToCheck = source.Select(x => Convert.ToDouble(x.Close)).ToArray();

			TicTacTec.TA.Library.Core.RetCode cmo = TicTacTec.TA.Library.Core.Cmo(0, source.Count() - 1, valuesToCheck, config.Period, out int outBegIdx, out int outNbElement, cmoValues);

			if (cmo == TicTacTec.TA.Library.Core.RetCode.Success)
			{
				return FixIndicatorOrdering(cmoValues.ToList(), outBegIdx, outNbElement);
			}

			throw new Exception("Could not calculate CMO");
		}

		public override dynamic Get (IEnumerable<decimal?> candles, IIndicatorOptions options = null)
		{
			throw new NotImplementedException();
		}
	}
}
