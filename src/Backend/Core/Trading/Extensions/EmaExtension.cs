using Core.Trading.Indicators;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Core.TypeCodes;

namespace Core.Trading.Extensions
{
	public static class EmaExtension
	{
		public static List<decimal?> Ema (this IEnumerable<ICandle> candles, int period, CandleVariableCode type)
		{
			IDictionary<string, object> options = new Dictionary<string, object>()
			{
				[nameof(period)] = period,
				[nameof(type)] = type
			}; 
			
			Ema ema = new Ema();
			return ema.Get(candles.ToArray(), options);
		}

		public static List<decimal?> Ema (this IEnumerable<decimal?> candles, int period)
		{
			Dictionary<string, object> options = new Dictionary<string, object>()
			{
				[nameof(period)] = period
			}; 
			
			Ema ema = new Ema();
			return ema.Get(candles.ToArray(), options);
		}
		
		public static List<decimal?> Ema (this IEnumerable<decimal> candles, int period)
		{
			Dictionary<string, object> options = new Dictionary<string, object>()
			{
				[nameof(period)] = period
			}; 
			
			IEnumerable<decimal?> values = candles.Select(x => (decimal?)x);
			Ema ema = new Ema();
			return ema.Get(values.ToArray(), options);
		}
	}
}
