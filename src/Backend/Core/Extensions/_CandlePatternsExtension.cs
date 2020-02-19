using System.Collections.Generic;
using Contracts;
using Core.Trading.Indicators;
using Core.Trading.Indicators.Options;
using Core.TypeCodes;

namespace Core.Trading.Extensions
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
