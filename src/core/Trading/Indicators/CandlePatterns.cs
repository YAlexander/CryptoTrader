using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Extensions;
using core.Trading.Indicators.Options;
using core.TypeCodes;

namespace core.Trading.Indicators
{
	public class CandlePatterns : BaseIndicator
	{
		public override string Name => nameof(CandlePatterns);

		public override dynamic Get (IEnumerable<ICandle> source, IIndicatorOptions options = null)
		{
			CandlePatternsOptions config = options != null ? (CandlePatternsOptions)options.Options : new CandlePatternsOptions(0.05m);

			List<CandlePatternCode> result = new List<CandlePatternCode>();

			List<decimal> open = source.Open();
			List<decimal> close = source.Close();
			List<decimal> high = source.High();
			List<decimal> low = source.Low();

			for (int i = 0; i < source.Count(); i++)
			{
				if (Math.Abs(open[i] - close[i]) <= (high[i] - low[i]) * config.DojiSize)
				{
					result.Add(CandlePatternCode.DOJI);
					continue;
				}

				if (high[i] - low[i] > 3 * (open[i] - close[i]) && (close[i] - low[i]) / (.001m + high[i] - low[i]) > 0.6m && (open[i] - low[i]) / (.001m + high[i] - low[i]) > 0.6m)
				{
					result.Add(CandlePatternCode.BULLISH_HAMMER);
					continue;
				}

				if (high[i] - low[i] > 3 * (open[i] - close[i]) && (high[i] - close[i]) / (.001m + high[i] - low[i]) > 0.6m && (high[i] - open[i]) / (.001m + high[i] - low[i]) > 0.6m)
				{
					result.Add(CandlePatternCode.BEARISH_INVERTED_HAMMER);
					continue;
				}

				// These patterns require at least 2 data points
				if (i > 0)
				{
					if (open[i - 1] < close[i - 1] && open[i] > close[i - 1] && high[i] - Math.Max(open[i], close[i]) >= Math.Abs(open[i] - close[i]) * 3 && Math.Min(close[i], open[i]) - low[i] <= Math.Abs(open[i] - close[i]))
					{
						result.Add(CandlePatternCode.SHOOTING_STAR);
						continue;
					}

					if (close[i - 1] > open[i - 1] && open[i] > close[i] && open[i] <= close[i - 1] && open[i - 1] <= close[i] && open[i] - close[i] < close[i - 1] - open[i - 1])
					{
						result.Add(CandlePatternCode.BEARISH_HARAMI);
						continue;
					}

					if (open[i - 1] > close[i - 1] && close[i] > open[i] && close[i] <= open[i - 1] && close[i - 1] <= open[i] && close[i] - open[i] < open[i - 1] - close[i - 1])
					{
						result.Add(CandlePatternCode.BULLISH_HARAMI);
						continue;
					}

					if (close[i - 1] > open[i - 1] && open[i] > close[i] && open[i] >= close[i - 1] && open[i - 1] >= close[i] && open[i] - close[i] > close[i - 1] - open[i - 1])
					{
						result.Add(CandlePatternCode.BEARISH_ENGULFING);
						continue;
					}

					if (open[i - 1] > close[i - 1] && close[i] > open[i] && close[i] >= open[i - 1] && close[i - 1] >= open[i] && close[i] - open[i] > open[i - 1] - close[i - 1])
					{
						result.Add(CandlePatternCode.BULLISH_ENGULFING);
						continue;
					}

					if (open[i - 1] > close[i - 1] && open[i] >= open[i - 1] && close[i] > open[i])
					{
						result.Add(CandlePatternCode.BULLISH_KICKER);
						continue;
					}

					if (open[i - 1] < close[i - 1] && open[i] <= open[i - 1] && close[i] <= open[i])
					{
						result.Add(CandlePatternCode.BEARISH_KICKER);
						continue;
					}

					if (close[i - 1] > open[i - 1] && (close[i - 1] + open[i - 1]) / 2 > close[i] && open[i] > close[i] && open[i] > close[i - 1] && close[i] > open[i - 1] && (open[i] - close[i]) / (.001m + (high[i] - low[i])) > 0.6m)
					{
						result.Add(CandlePatternCode.BEARISH_DARK_CLOUD_COVER);
						continue;
					}

					// These patterns require at least 3 data points
					if (i > 1)
					{
						if (close[i - 2] < open[i - 2] && Math.Max(open[i - 1], close[i - 1]) < close[i - 2] && open[i] > Math.Max(open[i - 1], close[i - 1]) && close[i] > open[i])
						{
							result.Add(CandlePatternCode.BULLISH_MORNING_STAR);
							continue;
						}

						if (close[i - 2] > open[i - 2] && Math.Min(open[i - 1], close[i - 1]) > close[i - 2] && open[i] < Math.Min(open[i - 1], close[i - 1]) && close[i] < open[i])
						{
							result.Add(CandlePatternCode.BEARISH_EVENING_STAR);
							continue;
						}

						if (high[i] - low[i] > 4 * (open[i] - close[i]) && (close[i] - low[i]) / (.001m + high[i] - low[i]) >= 0.75m && (open[i] - low[i]) / (.001m + high[i] - low[i]) >= 0.75m && high[i - 1] < open[i] && high[i - 2] < open[i])
						{
							result.Add(CandlePatternCode.BEARISH_HANGING_MAN);
							continue;
						}
					}

					if (i > 9)
					{
						decimal upper = high.Skip(i - 10).Take(10).OrderByDescending(x => x).ToList()[1];
						if (close[i - 1] < open[i - 1] && open[i] < low[i - 1] && close[i] > close[i - 1] + (open[i - 1] - close[i - 1]) / 2 && close[i] < open[i - 1])
						{
							result.Add(CandlePatternCode.PIERCING_LINE);
							continue;
						}

						decimal lower = high.Skip(i - 10).Take(10).OrderBy(x => x).ToList()[1];
						if (low[i] == open[i] && open[i] < lower && open[i] < close[i] && close[i] > (high[i - 1] - low[i - 1]) / 2 + low[i - 1])
						{
							result.Add(CandlePatternCode.BULLISH_BELT);
							continue;
						}
					}
				}

				result.Add(null);
			}

			return result;
		}

		public override dynamic Get (IEnumerable<decimal?> candles, IIndicatorOptions options = null)
		{
			throw new NotImplementedException();
		}
	}
}
