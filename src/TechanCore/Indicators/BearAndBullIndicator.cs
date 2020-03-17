using System;
using TechanCore.Helpers;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators
{
	public class BearAndBullIndicator : BaseIndicator<EmptyOption, SeriesIndicatorResult>
	{
		public override string Name => "Bear And Bull (B&B) Indicator";

		public override SeriesIndicatorResult Get(ICandle[] source, EmptyOption options)
		{
			decimal[] values = source.Close();

			decimal?[] result = new decimal?[source.Length];

			for (int i = 2; i < values.Length; i++)
			{
				decimal current = values[i];
				decimal previous = values[i - 1];
				decimal prior = values[i - 2];

				if (current > previous && previous > prior)
				{
					result[i] = 1;
				}
				else if (current < previous && previous < prior)
				{
					result[i] = -1;
				}
				else
				{
					result[i] = 0;
				}
			}

			return new SeriesIndicatorResult { Result = result };
		}

		public override SeriesIndicatorResult Get(decimal[] source, EmptyOption options)
		{
			throw new NotImplementedException();
		}

		public override SeriesIndicatorResult Get(decimal?[] source, EmptyOption options)
		{
			throw new NotImplementedException();
		}
	}
}