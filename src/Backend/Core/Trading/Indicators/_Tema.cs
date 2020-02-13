using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Trading;
using Contracts.TypeCodes;
using Core.Trading.Indicators.Options;
using Core.TypeCodes;

namespace Core.Trading.Indicators
{
	public class Tema : BaseIndicator
	{
		public override string Name => nameof(Tema);

		public override dynamic Get (IEnumerable<ICandle> source, IIndicatorOptions options = null)
		{
			TemaOptions config = options != null ? (TemaOptions)options.Options : new TemaOptions(30, CandleVariableCode.Close);

			double[] temaValues = new double[source.Count()];
			double[] valuesToCheck;

			if(config.Type == (ICandleVariableCode)CandleVariableCode.Open)
			{
				valuesToCheck = source.Select(x => Convert.ToDouble(x.Open)).ToArray();

			} 
			else if (config.Type == (ICandleVariableCode)CandleVariableCode.Low)
			{
				valuesToCheck = source.Select(x => Convert.ToDouble(x.Low)).ToArray();
			}
			else if (config.Type == (ICandleVariableCode)CandleVariableCode.High)
			{
				valuesToCheck = source.Select(x => Convert.ToDouble(x.High)).ToArray();
			}
			else
			{
				valuesToCheck = source.Select(x => Convert.ToDouble(x.Close)).ToArray();
			}

			TicTacTec.TA.Library.Core.RetCode tema = TicTacTec.TA.Library.Core.Tema(0, source.Count() - 1, valuesToCheck, config.Period, out int outBegIdx, out int outNbElement, temaValues);

			if (tema == TicTacTec.TA.Library.Core.RetCode.Success)
			{
				return FixIndicatorOrdering(temaValues.ToList(), outBegIdx, outNbElement);
			}

			throw new Exception("Could not calculate TEMA");
		}

		public override dynamic Get (IEnumerable<decimal?> candles, IIndicatorOptions options = null)
		{
			throw new NotImplementedException();
		}
	}
}
