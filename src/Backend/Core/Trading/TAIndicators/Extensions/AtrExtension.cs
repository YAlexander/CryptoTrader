using System.Collections.Generic;
using System.Linq;
using Contracts;
using core.Trading.TAIndicators;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators.Extensions
{
	public static class AtrExtension
	{
		public static DefaultIndicatorResult Atr (this IEnumerable<ICandle> source, int period)
		{
			AtrIndicator atr = new AtrIndicator();
			AtrOptions options = new AtrOptions() {Period = period};
			
			return atr.Get(source.ToArray(), options);
		}
	}
}