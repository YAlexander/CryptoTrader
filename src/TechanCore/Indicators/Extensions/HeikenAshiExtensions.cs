using System;
using System.Collections.Generic;
using System.Linq;
using TechanCore.Enums;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
{
	public static class HeikenAshiExtensions
	{
		public static HeikenAshiResult HeikenAshi(this IEnumerable<ICandle> source, MaTypes type, int period, bool isSmoothed)
		{
			HeikenAshiIndicator heikenAshi = new HeikenAshiIndicator();
			HeikenAshiOptions heikenOptions = new HeikenAshiOptions();
			heikenOptions.IsSmoothed = isSmoothed;
			heikenOptions.MaPeriod = period;
			heikenOptions.MaType = type;

			return heikenAshi.Get(source.ToArray(), heikenOptions);
		}
	}
}
