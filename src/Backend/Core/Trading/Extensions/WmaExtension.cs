using System.Collections.Generic;
using System.Linq;
using Contracts;
using Core.Trading.Indicators;
using Core.TypeCodes;

namespace Core.Trading.Extensions
{
	public static class WmaExtension
	{
		public static List<decimal?> Wma (this IEnumerable<ICandle> candles, int period, CandleVariableCode type)
		{
			IDictionary<string, object> options = new Dictionary<string, object>()
			{
				[nameof(period)] = period,
				[nameof(type)] = type
			};

			Wma wma = new Wma();
			return wma.Get(candles.ToArray(), options);
		}

		public static List<decimal?> Wma (this IEnumerable<decimal?> candles, int period)
		{
			IDictionary<string, object> options = new Dictionary<string, object>()
			{
				[nameof(period)] = period,
			};

			Wma wma = new Wma();
			return wma.Get(candles.ToArray(), options);
		}
		
		public static List<decimal?> Wma (this IEnumerable<decimal> candles, int period)
		{
			Dictionary<string, object> options = new Dictionary<string, object>()
			{
				[nameof(period)] = period
			}; 
			
			IEnumerable<decimal?> values = candles.Select(x => (decimal?)x);
			Wma wma = new Wma();
			return wma.Get(values.ToArray(), options);
		}
	}
}
