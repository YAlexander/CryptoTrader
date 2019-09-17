using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Indicators;
using core.Trading.Indicators.Options;
using core.Trading.Models;
using core.TypeCodes;

namespace Core.Trading.Indicators
{
	public class StochRsi : BaseIndicator
	{
		public override string Name => nameof(StochRsi);

		public override dynamic Get (IEnumerable<ICandle> source, IIndicatorOptions options = null)
		{
			StochRsiOptions config = options != null ? (StochRsiOptions)options.Options : new StochRsiOptions(14, CandleVariableCode.CLOSE, 5, 3, TicTacTec.TA.Library.Core.MAType.Sma);

			double[] kValues = new double[source.Count()];
			double[] dValues = new double[source.Count()];
			double[] valuesToCheck;

			if (config.Type == (ICandleVariableCode)CandleVariableCode.OPEN)
			{
				valuesToCheck = source.Select(x => Convert.ToDouble(x.Open)).ToArray();
			}
			else if (config.Type == (ICandleVariableCode)CandleVariableCode.LOW)
			{
				valuesToCheck = source.Select(x => Convert.ToDouble(x.Low)).ToArray();
			}
			else if (config.Type == (ICandleVariableCode)CandleVariableCode.HIGH)
			{
				valuesToCheck = source.Select(x => Convert.ToDouble(x.High)).ToArray();
			}
			else
			{
				valuesToCheck = source.Select(x => Convert.ToDouble(x.Close)).ToArray();
			}

			TicTacTec.TA.Library.Core.RetCode tema = TicTacTec.TA.Library.Core.StochRsi(0, source.Count() - 1, valuesToCheck, config.OptInTimePeriod, config.FastKPeriod, config.FastDPeriod, config.FastDmaType, out int outBegIdx, out int outNbElement, kValues, dValues);

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
