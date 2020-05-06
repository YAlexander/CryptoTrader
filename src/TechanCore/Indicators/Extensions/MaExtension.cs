using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechanCore.Enums;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
{
	public static class MaExtension
	{
		public static SeriesIndicatorResult Ma(this IEnumerable<ICandle> source, MaTypes maType, int period, CandleVariables type)
		{
			if (maType == MaTypes.EMA)
			{
				EmaIndicator ema = new EmaIndicator();
				EmaOptions options = new EmaOptions { Period = period, CandleVariable = type };

				return ema.Get(source.ToArray(), options);
			}
			else if (maType == MaTypes.WMA)
			{
				WmaIndicator wma = new WmaIndicator();
				WmaOptions options = new WmaOptions { Period = period, CandleVariable = type };

				return wma.Get(source.ToArray(), options);
			}
			else
			{
				SmaIndicator sma = new SmaIndicator();
				SmaOptions options = new SmaOptions { Period = period, CandleVariable = type };

				return sma.Get(source.ToArray(), options);
			}
		}

		public static SeriesIndicatorResult Ma(this IEnumerable<decimal> source, MaTypes maType, int period)
		{
			if (maType == MaTypes.EMA)
			{
				EmaIndicator ema = new EmaIndicator();
				EmaOptions options = new EmaOptions { Period = period, CandleVariable = null };

				return ema.Get(source.ToArray(), options);
			}
			else if (maType == MaTypes.WMA)
			{
				WmaIndicator wma = new WmaIndicator();
				WmaOptions options = new WmaOptions { Period = period, CandleVariable = null };

				return wma.Get(source.ToArray(), options);
			}
			else
			{
				SmaIndicator sma = new SmaIndicator();
				SmaOptions options = new SmaOptions { Period = period, CandleVariable = null };

				return sma.Get(source.ToArray(), options);
			}
		}

		public static SeriesIndicatorResult Ma(this IEnumerable<decimal?> source, MaTypes maType, int period)
		{
			if (maType == MaTypes.EMA)
			{
				EmaIndicator ema = new EmaIndicator();
				EmaOptions options = new EmaOptions { Period = period, CandleVariable = null };

				return ema.Get(source.ToArray(), options);
			}
			else if (maType == MaTypes.WMA)
			{
				WmaIndicator wma = new WmaIndicator();
				WmaOptions options = new WmaOptions { Period = period, CandleVariable = null };

				return wma.Get(source.ToArray(), options);
			}
			else
			{
				SmaIndicator sma = new SmaIndicator();
				SmaOptions options = new SmaOptions { Period = period, CandleVariable = null };

				return sma.Get(source.ToArray(), options);
			}
		}
	}
}
