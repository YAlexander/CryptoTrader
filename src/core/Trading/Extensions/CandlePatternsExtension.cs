using core.Abstractions;
using core.Trading.Indicators;
using core.Trading.Indicators.Options;
using core.TypeCodes;
using System.Collections.Generic;

namespace core.Trading.Extensions
{
	public static class CandlePatternsExtension
	{
		public static List<CandlePatternCode> CandlePatterns (this IEnumerable<ICandle> candles, decimal? dojiSize = null)
		{
			dojiSize ??= 0.05m;

			CandlePatternsOptions config = new CandlePatternsOptions(dojiSize.Value);
			CandlePatterns cp = new CandlePatterns();
			return cp.Get(candles, config);
		}
	}
}
