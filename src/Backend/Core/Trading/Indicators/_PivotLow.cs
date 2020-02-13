using Core.Trading.Indicators.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Trading;

namespace Core.Trading.Indicators
{

	public class PivotLow : BaseIndicator
	{
		public override string Name => nameof(PivotLow);

		public override dynamic Get (IEnumerable<ICandle> source, IIndicatorOptions options = null)
		{
			PivotLowOptions config = options != null ? (PivotLowOptions)options.Options : new PivotLowOptions(4, 2, false);

			List<decimal?> result = new List<decimal?>();

			for (int i = 0; i < source.Count(); i++)
			{
				if (i < config.BarsLeft + config.BarsRight)
				{
					result.Add(null);
					continue;
				}

				bool isPivot = true;
				List<ICandle> subSet = source.Skip(i - config.BarsLeft - config.BarsRight).Take(config.BarsLeft + config.BarsRight + 1).ToList();
				ICandle valueToCheck = subSet[config.BarsLeft];

				// Check if the [barsLeft] bars left of what we're checking all have lower highs or equal
				for (int leftPivot = 0; leftPivot < config.BarsLeft; leftPivot++)
				{
					if (subSet[leftPivot].Low < valueToCheck.Low)
					{
						isPivot = false;
						break;
					}
				}

				// If it's still a pivot by this point, carry on checking the right side...
				if (isPivot)
				{
					// If the [barsRight] right side all have lower highs, it's a pivot!
					for (int rightPivot = config.BarsLeft + 1; rightPivot < subSet.Count; rightPivot++)
					{
						if (subSet[rightPivot].Low <= valueToCheck.Low)
						{
							isPivot = false;
							break;
						}
					}

					// If it's a pivot
					if (isPivot)
					{
						result.Add(valueToCheck.Low);
					}
					else
					{
						result.Add(null);
					}
				}
				else
				{
					result.Add(null);
				}
			}

			if (config.FillNullValues)
			{
				return FillPivotNulls(result);
			}

			return result;
		}

		public override dynamic Get (IEnumerable<decimal?> candles, IIndicatorOptions options = null)
		{
			throw new NotImplementedException();
		}
	}
}
