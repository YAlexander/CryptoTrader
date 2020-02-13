using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Trading;

namespace Core.Trading.TAIndicators
{
	public abstract class BaseIndicator<T, TK> : IIndicator<T, TK> where T : IOptionsSet where TK: IResultSet
	{
		public abstract string Name { get; }

		public abstract TK Get (ICandle[] source, T options);
		public abstract TK Get(decimal[] source, T options);
		public abstract TK Get(decimal?[] source, T options);

		// protected List<decimal?> FixIndicatorOrdering (IEnumerable<double> items, int outBegIdx, int outNbElement)
		// {
		// 	List<decimal?> outValues = new List<decimal?>();
		// 	IEnumerable<double> validItems = items.Take(outNbElement);
		//
		// 	for (int i = 0; i < outBegIdx; i++)
		// 	{
		// 		outValues.Add(null);
		// 	}
		//
		// 	outValues.AddRange(validItems.Select(value => (decimal?) value));
		// 	return outValues;
		// }
		
		protected List<decimal?> FillPivotNulls (List<decimal?> result)
		{
			List<(decimal, int)> values = new List<(decimal, int)>();
			int nullCounter = 0;

			foreach (decimal? item in result)
			{
				if (item.HasValue)
				{
					values.Add((item.Value, nullCounter));
					nullCounter = 0;
				}
				else
				{
					nullCounter += 1;
				}
			}

			List<decimal?> finalList = new List<decimal?>();
			bool isFirst = true;

			for (int i = 0; i < values.Count; i++)
			{
				if (isFirst)
				{
					for (int j = 0; j < values[i].Item2; j++)
						finalList.Add(null);

					finalList.Add(values[i].Item1);

					isFirst = false;
				}
				else
				{
					(decimal, int) current = values[i];
					(decimal, int) previous = values[i - 1];
					int count = current.Item2;

					for (int x = 1; x <= count; x++)
					{
						if (current.Item1 > previous.Item1)
						{
							decimal amountToUse = (current.Item1 - previous.Item1) / (count + 1);
							finalList.Add(Math.Round(previous.Item1 + amountToUse * x, 8));
						}
						else
						{
							decimal amountToUse = (previous.Item1 - current.Item1) / (count + 1);
							finalList.Add(Math.Round(previous.Item1 - amountToUse * x, 8));
						}
					}

					finalList.Add(current.Item1);
				}
			}

			int finalCount = finalList.Count;
			for (int i = 0; i < result.Count - finalCount; i++)
			{
				finalList.Add(null);
			}

			return finalList;
		}
	}
}
