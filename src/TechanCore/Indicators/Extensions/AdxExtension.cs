using System.Collections.Generic;
using System.Linq;
using Contracts;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
{
	public static class AdxExtension
	{
		public static AdxResult Adx (this IEnumerable<ICandle> source, int period)
		{
			AverageDirectionalIndexIndicator atr = new AverageDirectionalIndexIndicator();
			AdxOptions options = new AdxOptions { Period = period };
			
			return atr.Get(source.ToArray(), options);
		}
	}
}