using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Trading;
using Core.Trading.Indicators.Options;

namespace Core.Trading.Indicators
{
	public class Mom : BaseIndicator
	{
		public override string Name => nameof(Mom);

		public override dynamic Get (IEnumerable<ICandle> source, IIndicatorOptions options = null)
		{
			MomOptions config = options != null ? (MomOptions)options.Options : new MomOptions(10);

			double[] momValues = new double[source.Count()];
			double[] closes = source.Select(x => Convert.ToDouble(x.Close)).ToArray();

			TicTacTec.TA.Library.Core.RetCode mom = TicTacTec.TA.Library.Core.Mom(0, source.Count() - 1, closes, config.Period, out int outBegIdx, out int outNbElement, momValues);

			if (mom == TicTacTec.TA.Library.Core.RetCode.Success)
			{
				return FixIndicatorOrdering(momValues.ToList(), outBegIdx, outNbElement);
			}

			throw new Exception("Could not calculate MOM!");
		}

		public override dynamic Get (IEnumerable<decimal?> candles, IIndicatorOptions options = null)
		{
			throw new NotImplementedException();
		}
	}
}
