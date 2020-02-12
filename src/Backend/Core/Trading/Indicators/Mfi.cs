using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Trading;
using Core.Trading.Indicators.Options;

namespace Core.Trading.Indicators
{
	public class Mfi : BaseIndicator
	{
		public override string Name => nameof(Mfi);

		public override dynamic Get (IEnumerable<ICandle> source, IIndicatorOptions options = null)
		{
			MfiOptions config = options != null ? (MfiOptions)options.Options : new MfiOptions(14);
			double[] mfiValues = new double[source.Count()];

			double[] highs = source.Select(x => Convert.ToDouble(x.High)).ToArray();
			double[] lows = source.Select(x => Convert.ToDouble(x.Low)).ToArray();
			double[] closes = source.Select(x => Convert.ToDouble(x.Close)).ToArray();
			double[] volumes = source.Select(x => Convert.ToDouble(x.Volume)).ToArray();

			TicTacTec.TA.Library.Core.RetCode mfi = TicTacTec.TA.Library.Core.Mfi(0, source.Count() - 1, highs, lows, closes, volumes, config.Period, out int outBegIdx, out int outNbElement, mfiValues);

			if (mfi == TicTacTec.TA.Library.Core.RetCode.Success)
			{
				return FixIndicatorOrdering(mfiValues.ToList(), outBegIdx, outNbElement);
			}

			throw new Exception("Could not calculate MFI");
		}

		public override dynamic Get (IEnumerable<decimal?> candles, IIndicatorOptions options = null)
		{
			throw new NotImplementedException();
		}
	}
}
