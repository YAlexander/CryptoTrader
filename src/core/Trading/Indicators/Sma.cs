using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Indicators.Options;
using core.TypeCodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace core.Trading.Indicators
{
	public class Sma : BaseIndicator
	{
		public override string Name => nameof(Sma);

		public override dynamic Get (IEnumerable<ICandle> source, IIndicatorOptions options = null)
		{
			SmaOptions config = options != null ? (SmaOptions)options.Options : new SmaOptions(30, CandleVariableCode.CLOSE);
			IEnumerable<decimal?> values;

			if(config.Type == (ICandleVariableCode)CandleVariableCode.OPEN)
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
			SmaOptions config = options != null ? (SmaOptions)options.Options : new SmaOptions(30, CandleVariableCode.CLOSE);

			double[] smaValues = new double[source.Count()];
			List<double?> outValues = new List<double?>();

			int sourceNullCount = source.Count(x => x == null);
			double[] sourceFix = source.Where(x => x != null).Select(x => Convert.ToDouble(x)).ToArray();

			TicTacTec.TA.Library.Core.RetCode sma = TicTacTec.TA.Library.Core.Sma(0, source.Count() - 1 - sourceNullCount, sourceFix, config.Period, out int outBegIdx, out int outNbElement, smaValues);

			if (sma == TicTacTec.TA.Library.Core.RetCode.Success)
			{
				List<decimal?> smas = FixIndicatorOrdering(smaValues.ToList(), outBegIdx, outNbElement);

				for (int i = 0; i < sourceNullCount; i++)
				{
					smas.Insert(0, null);
				}

				return smas;
			}

			throw new Exception("Could not calculate SMA!");
		}
	}
}
