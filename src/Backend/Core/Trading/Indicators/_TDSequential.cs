using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Trading;
using Core.Trading.Extensions;
using Core.Trading.Models;

namespace Core.Trading.Indicators
{

	public class TdSequential : BaseIndicator
	{
		public override string Name => nameof(TdSequential);

		public override dynamic Get (IEnumerable<ICandle> source, IIndicatorOptions options = null)
		{
			List<TdSeqItem> result = new List<TdSeqItem>();
			List<decimal> close = source.Close();
			List<int> td = new List<int>();
			List<int> ts = new List<int>();
			List<int?> tdUps = new List<int?>();
			List<int?> tdDowns = new List<int?>();

			for (int i = 0; i < source.Count(); i++)
			{
				if (i < 4)
				{
					td.Add(0);
					ts.Add(0);
					result.Add(null);
					continue;
				}

				int currentTd = close[i] > close[i - 4] ? td[i - 1] + 1 : 0;
				int currentTs = close[i] < close[i - 4] ? ts[i - 1] + 1 : 0;
				td.Add(currentTd);
				ts.Add(currentTs);

				int? tdUp = currentTd - ValueWhenPreviousLower(td);
				int? tdDown = currentTs - ValueWhenPreviousLower(ts);

				tdUps.Add(tdUp);
				tdDowns.Add(tdDown);

				if (tdUp > 0 && tdUp <= 9)
				{
					result.Add(new TdSeqItem { Value = tdUp, IsGreen = true });
				}
				else if (tdDown > 0 && tdDown <= 9)
				{
					result.Add(new TdSeqItem { Value = tdDown, IsGreen = false });
				}
				else
				{
					result.Add(new TdSeqItem { Value = null, IsGreen = true });
				}
			}

			return result;
		}

		private static int? ValueWhenPreviousLower (List<int> values)
		{
			int counter = 0;

			for (int i = 0; i < values.Count; i++)
			{
				if (i == 0)
				{
					continue;
				}

				if (values[i] < values[i - 1] && counter == 1)
				{
					return values[i];
				}
				else if (values[i] < values[i - 1])
				{
					counter += 1;
				}
			}

			return null;
		}

		public override dynamic Get (IEnumerable<decimal?> candles, IIndicatorOptions options = null)
		{
			throw new NotImplementedException();
		}
	}
}
