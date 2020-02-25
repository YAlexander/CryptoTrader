using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Indicators;
using core.Trading.Indicators.Options;
using core.TypeCodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Trading.Indicators
{
	public class Ema : BaseIndicator
	{
		public override string Name => nameof(Ema);

		public override dynamic Get (IEnumerable<ICandle> source, IIndicatorOptions options = null)
		{
			EmaOptions config = options != null ? (EmaOptions)options.Options : new EmaOptions(30, CandleVariableCode.CLOSE);
			IEnumerable<decimal?> values;

			if (config.Type == (ICandleVariableCode)CandleVariableCode.OPEN)
			{
				values = source.Select(x => (decimal?)x.Open);
			}
			else if (config.Type == (ICandleVariableCode)CandleVariableCode.LOW)
			{
				values = source.Select(x => (decimal?)x.Low);
			}
			else if (config.Type == (ICandleVariableCode)CandleVariableCode.HIGH)
			{
				values = source.Select(x => (decimal?)x.High);
			}
			else
			{
				values = source.Select(x => (decimal?)x.Close);
			}

			return Get(values, options);
		}

		public override dynamic Get (IEnumerable<decimal?> source, IIndicatorOptions options = null)
		{
			EmaOptions config = options != null ? (EmaOptions)options.Options : new EmaOptions(30, CandleVariableCode.CLOSE);

			double[] emaValues = new double[source.Count()];
			List<double?> outValues = new List<double?>();

			double[] sourceFix = source.Select(x => x.HasValue ? Convert.ToDouble(x) : 0).ToArray();

			TicTacTec.TA.Library.Core.RetCode ema = TicTacTec.TA.Library.Core.Ema(0, source.Count() - 1, sourceFix, config.Period, out int outBegIdx, out int outNbElement, emaValues);

			if (ema == TicTacTec.TA.Library.Core.RetCode.Success)
			{
				return FixIndicatorOrdering(emaValues.ToList(), outBegIdx, outNbElement);
			}

			throw new Exception("Could not calculate EMA!");
		}
	}
}
