using Core.Trading.Indicators.Options;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Trading;

namespace Core.Trading.Indicators
{

	public class PivotHigh : BaseIndicator
	{
		public override string Name => nameof(PivotHigh);

		public override dynamic Get (IEnumerable<ICandle> source, IIndicatorOptions options = null)
		{
			PivotHighOptions config = options != null ? (PivotHighOptions)options.Options : new PivotHighOptions(4, 2, false);

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
				var valueToCheck = subSet[config.BarsLeft];

				// Check if the [barsLeft] bars left of what we're checking all have lower highs or equal
				for (int leftPivot = 0; leftPivot < config.BarsLeft; leftPivot++)
				{
					if (subSet[leftPivot].High > valueToCheck.High)
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
						if (subSet[rightPivot].High >= valueToCheck.High)
						{
							isPivot = false;
							break;
						}
					}

					// If it's a pivot
					if (isPivot)
					{
						result.Add(valueToCheck.High);
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
			throw new System.NotImplementedException();
		}
	}
}
