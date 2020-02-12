using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Core.TypeCodes;

namespace Core.Trading.Indicators
{
	public class Sma : BaseIndicator
	{
		public override string Name => nameof(Sma);

		public override dynamic Get (ICandle[] source, IDictionary<string, object> options)
		{
			CandleVariableCode type = (CandleVariableCode)options["type"];

			IEnumerable<decimal?> values;

			if(type == CandleVariableCode.Open)
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
			double[] smaValues = new double[source.Count()];

			int sourceNullCount = source.Count(x => x == null);
			double[] sourceFix = source.Where(x => x != null).Select(x => Convert.ToDouble(x)).ToArray();

			TicTacTec.TA.Library.Core.RetCode sma = TicTacTec.TA.Library.Core.Sma(0, source.Count() - 1 - sourceNullCount, sourceFix, period, out int outBegIdx, out int outNbElement, smaValues);

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
