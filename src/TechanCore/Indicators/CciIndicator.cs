using System;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators
{
	public class CciIndicator : BaseIndicator<CciOptions, SeriesIndicatorResult>
	{
		public override string Name { get; } = "Commodity Channel Index (CCI) Indicator";

		public override SeriesIndicatorResult Get(ICandle[] source, CciOptions options)
		{
			decimal?[] cciSourceValues = new decimal?[source.Length];
			decimal?[] meanDeviationList = new decimal?[source.Length];
			decimal?[] result = new decimal?[source.Length];

			for (int i = 0; i < source.Length; i++)
			{
				cciSourceValues[i] = (source[i].High + source[i].Low + source[i].Close) / 3;
			}

			decimal?[] smaList = cciSourceValues.Sma(options.Period).Result;

			for (int i = 0; i < source.Length - 1; i++)
			{
				if (i >= options.Period)
				{
					decimal? total = 0m;
					for (int j = i; j >= i - options.Period; j--)
					{
						total += Math.Abs(smaList[i].Value - cciSourceValues[j].Value);
					}

					meanDeviationList[i] = total / options.Period;
					result[i] = (cciSourceValues[i] - smaList[i]) / (0.015m * meanDeviationList[i]);
				}
				else
				{
					meanDeviationList[i] = null;
					result[i] = null;
				}
			}

			return new SeriesIndicatorResult { Result = result };
		}

		public override SeriesIndicatorResult Get(decimal[] source, CciOptions options)
		{
			throw new NotImplementedException();
		}

		public override SeriesIndicatorResult Get(decimal?[] source, CciOptions options)
		{
			throw new NotImplementedException();
		}
	}
}