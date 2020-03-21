using System;
using System.Collections.Generic;
using System.Linq;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;

namespace TechanCore.Helpers
{
	public static class CandleExtensions
	{
		public static IEnumerable<ICandle> HeikenAshi(this ICandle[] source)
		{
			if (source == null || !source.Any()) return source;
			ICandle[] ashiCandles = new ICandle[source.Length];
			source.CopyTo(ashiCandles, 0);

			for (int index = 0; index < source.Length; index++)
			{
				if (index == 0)
				{
					ashiCandles[index].Time = source[index].Time;
					ashiCandles[index].Volume = source[index].Volume;
					ashiCandles[index].Trades = source[index].Trades;
					ashiCandles[index].Open = source[index].Open;
					ashiCandles[index].Close = source[index].Close;
					ashiCandles[index].High = source[index].High;
					ashiCandles[index].Low = source[index].Low;
				}
				else
				{
					ashiCandles[index].Time = source[index].Time;
					ashiCandles[index].Volume = source[index].Volume;
					ashiCandles[index].Trades = source[index].Trades;
					ashiCandles[index].Open = (source[index - 1].Open + source[index - 1].Close) / 2;
					ashiCandles[index].Close = (source[index].Open + source[index].Close + source[index].Low + source[index].High) / 4;
					ashiCandles[index].High = new[] {source[index].Open, source[index].Close, source[index].High}.Max();
					ashiCandles[index].Low = new[] {source[index].Open, source[index].Close, source[index].Low}.Min();
				}
			}

			return ashiCandles;
		}

		public static IEnumerable<ICandle> HeikenAshiSmoothed(this ICandle[] source, MaTypes type, int maPeriod)
		{
			if (source == null || !source.Any()) return source;

			ICandle[] ashiCandles = new ICandle[source.Length];
			source.CopyTo(ashiCandles, 0);

			decimal?[] close;
			decimal?[] open;
			decimal?[] low;
			decimal?[] high;

			switch (type)
			{
				case MaTypes.SMA:
					close = source.Close().Sma(maPeriod).Result;
					open = source.Open().Sma(maPeriod).Result;
					low = source.Low().Sma(maPeriod).Result;
					high = source.High().Sma(maPeriod).Result;
					break;
				case MaTypes.EMA:
					close = source.Close().Ema(maPeriod).Result;
					open = source.Open().Ema(maPeriod).Result;
					low = source.Low().Ema(maPeriod).Result;
					high = source.High().Ema(maPeriod).Result;
					break;
				case MaTypes.WMA:
					close = source.Close().Wma(maPeriod).Result;
					open = source.Open().Wma(maPeriod).Result;
					low = source.Low().Wma(maPeriod).Result;
					high = source.High().Wma(maPeriod).Result;
					break;
				default:
					throw new Exception($"Unsupported MA type {type}");
			}

			for (int index = 0; index < source.Length; index++)
			{
				if (index == 0)
				{
					ashiCandles[index].Time = source[index].Time;
					ashiCandles[index].Volume = source[index].Volume;
					ashiCandles[index].Trades = source[index].Trades;
					ashiCandles[index].Open = open?[index] ?? 0;
					ashiCandles[index].Close = close?[index] ?? 0;
					ashiCandles[index].High = high?[index] ?? 0;
					ashiCandles[index].Low = low?[index] ?? 0;
				}
				else
				{
					ashiCandles[index].Time = source[index].Time;
					ashiCandles[index].Volume = source[index].Volume;
					ashiCandles[index].Trades = source[index].Trades;
					ashiCandles[index].Open = (open?[index - 1] ?? 0m + open?[index - 1] ?? 0) / 2;
					ashiCandles[index].Close = (close?[index - 1] ??  0 + close?[index - 1] ??  0 + close?[index - 1] ?? 0 + close?[index - 1] ?? 0) / 4;
					ashiCandles[index].High = new[] {high?[index - 1] ?? 0, high?[index - 1] ?? 0, high?[index - 1] ?? 0}.Max();
					ashiCandles[index].Low =  new[] {low?[index - 1] ?? 0, low?[index - 1] ?? 0, low?[index - 1] ?? 0}.Min();
				}
			}

			return ashiCandles;
		}
	}
}