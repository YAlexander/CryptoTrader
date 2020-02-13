using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Core.TypeCodes;

namespace Core.Trading.Indicators
{
	public class Wma : BaseIndicator
	{
		public override string Name => nameof(Wma);

		public override dynamic Get (ICandle[] source, IDictionary<string, object> options)
		{
			IEnumerable<decimal?> values;
			CandleVariableCode type = (CandleVariableCode)options["type"];

			if (type == CandleVariableCode.Open)
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

		public override dynamic Get (decimal?[] source, IDictionary<string, object> options)
		{
			int period = (int)options["period"];
			double[] wmaValues = new double[source.Count()];

			double[] sourceFix = source.Select(x => x.HasValue ? Convert.ToDouble(x) : 0).ToArray();

			TicTacTec.TA.Library.Core.RetCode wma = TicTacTec.TA.Library.Core.Wma(0, source.Count() - 1, sourceFix, period, out int outBegIdx, out int outNbElement, wmaValues);

			if (wma == TicTacTec.TA.Library.Core.RetCode.Success)
			{
				return FixIndicatorOrdering(wmaValues.ToList(), outBegIdx, outNbElement);
			}

			throw new Exception("Could not calculate WMA!");
		}
	}
}
