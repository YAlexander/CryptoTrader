using System.Collections.Generic;
using System.Linq;
using Contracts;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators.Extensions
{
	public static class RsiExtension
	{
		public static SeriesIndicatorResult Rsi (this IEnumerable<ICandle> source, int period)
		{
			RsiIndicator rsi = new RsiIndicator();
			RsiOptions options = new RsiOptions() { Period = period };
			
			return rsi.Get(source.ToArray(), options);
		}
	}
}