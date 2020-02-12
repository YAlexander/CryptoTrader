using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Core.TypeCodes;

namespace Core.Trading.Indicators
{
	public class Ema : BaseIndicator
	{
		public override string Name => nameof(Ema);

		public override dynamic Get (ICandle[] source, dynamic options)
		{
			IEnumerable<decimal?> values;
			
			if (options.type == CandleVariableCode.Open)
			{
				values = source.Select(x => (decimal?)x.Open);
			}
			else if (type == CandleVariableCode.Low)
			{
				values = source.Select(x => (decimal?)x.Low);
			}
			else if (type == CandleVariableCode.High)
			{
				values = source.Select(x => (decimal?)x.High);
			}
			else
			{
				values = source.Select(x => (decimal?)x.Close);
			}

			return Get(values.ToArray(), options);
		}
		
		public override dynamic Get(decimal?[] source, IDictionary<string, object> options)
		{
			int period = (int)options["period"];
			
			double[] emaValues = new double[source.Count()];
			double[] sourceFix = source.Select(x => x.HasValue ? Convert.ToDouble(x) : 0).ToArray();

			TicTacTec.TA.Library.Core.RetCode ema = TicTacTec.TA.Library.Core.Ema(0, source.Count() - 1, sourceFix, period, out int outBegIdx, out int outNbElement, emaValues);

			if (ema == TicTacTec.TA.Library.Core.RetCode.Success)
			{
				return FixIndicatorOrdering(emaValues.ToList(), outBegIdx, outNbElement);
			}

			throw new Exception("Could not calculate EMA!");
		}
	}
}
