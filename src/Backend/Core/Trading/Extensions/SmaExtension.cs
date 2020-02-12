using System.Collections.Generic;
using System.Linq;
using Contracts;
using Core.Trading.Indicators;
using Core.TypeCodes;

namespace Core.Trading.Extensions
{
	public static class SmaExtension
	{
		public static List<decimal?> Sma (this IEnumerable<ICandle> candles, int period, CandleVariableCode type)
		{
			IDictionary<string, object> options = new Dictionary<string, object>()
			{
				[nameof(period)] = period,
				[nameof(type)] = type
			};

			Sma sma = new Sma();
			return sma.Get(candles.ToArray(), options);
		}

		public static List<decimal?> Sma (this IEnumerable<decimal?> candles, int period)
		{
			IDictionary<string, object> options = new Dictionary<string, object>()
			{
				[nameof(period)] = period,
			};

			Sma sma = new Sma();
			return sma.Get(candles.ToArray(), options);
		}
		
		public static List<decimal?> Sma (this IEnumerable<decimal> candles, int period)
		{
			Dictionary<string, object> options = new Dictionary<string, object>()
			{
				[nameof(period)] = period
			}; 
			
			IEnumerable<decimal?> values = candles.Select(x => (decimal?)x);
			Sma sma = new Sma();
			return sma.Get(values.ToArray(), options);
		}
	}
}
