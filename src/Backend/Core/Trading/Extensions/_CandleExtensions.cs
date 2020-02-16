using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.TAIndicators.Extensions;
using Persistence.Entities;

namespace Core.Trading.Extensions
{
	public static class CandleExtensions
	{
		public static List<decimal> High(this IEnumerable<ICandle> source) => source.Select(x => x.High).ToList();

		public static List<decimal> Low(this IEnumerable<ICandle> source) => source.Select(x => x.Low).ToList();

		public static List<decimal> Open(this IEnumerable<ICandle> source) => source.Select(x => x.Open).ToList();

		public static List<decimal> Close(this IEnumerable<ICandle> source) => source.Select(x => x.Close).ToList();

		public static List<decimal> Hl2(this IEnumerable<ICandle> source) =>
			source.Select(x => (x.High + x.Low) / 2).ToList();

		public static List<decimal> Hlc3(this IEnumerable<ICandle> source) =>
			source.Select(x => (x.High + x.Low + x.Close) / 3).ToList();

		public static IEnumerable<ICandle> HeikenAshi(this ICandle[] source)
		{
			if (source == null || !source.Any()) return source;

			List<ICandle> ashiCandles = new List<ICandle>();

			for (int index = 0; index < source.Length; index++)
			{
				if (index == 0)
				{
					ashiCandles.Add(new Candle()
					{
						Time = source[index].Time,
						Volume = source[index].Volume,
						Trades = source[index].Trades,
						Open = source[index].Open,
						Close = source[index].Close,
						High = source[index].High,
						Low = source[index].Low
					});
				}
				else
				{
					ashiCandles.Add(new Candle()
					{
						Time = source[index].Time,
						Volume = source[index].Volume,
						Trades = source[index].Trades,
						Open = (source[index - 1].Open + source[index - 1].Close) / 2,
						Close = (source[index].Open + source[index].Close + source[index].Low + source[index].High) / 4,
						High = new[] {source[index].Open, source[index].Close, source[index].High}.Max(),
						Low = new[] {source[index].Open, source[index].Close, source[index].Low}.Min()
					});
				}
			}

			return ashiCandles;
		}


		public static IEnumerable<ICandle> HeikenAshiSmoothed(this ICandle[] source, MaTypes type, int maPeriod)
		{
			if (source == null || !source.Any()) return source;

			List<ICandle> ashiCandles = new List<ICandle>();

			List<decimal?> close;
			List<decimal?> open;
			List<decimal?> low;
			List<decimal?> high;

			switch (type)
			{
				case MaTypes.SMA:
					close = source.Select(x => x.Close).Sma(maPeriod);
					open = source.Select(x => x.Open).Sma(maPeriod);
					low = source.Select(x => x.Low).Sma(maPeriod);
					high = source.Select(x => x.High).Sma(maPeriod);
					break;
				case MaTypes.EMA:
					close = source.Select(x => x.Close).Ema(maPeriod);
					open = source.Select(x => x.Open).Ema(maPeriod);
					low = source.Select(x => x.Low).Ema(maPeriod);
					high = source.Select(x => x.High).Ema(maPeriod);
					break;
				case MaTypes.WMA:
					close = source.Select(x => x.Close).Wma(maPeriod);
					open = source.Select(x => x.Open).Wma(maPeriod);
					low = source.Select(x => x.Low).Wma(maPeriod);
					high = source.Select(x => x.High).Wma(maPeriod);
					break;
				default:
					throw new Exception($"Unsupported MA type {type}");
			}

			for (int index = 0; index < source.Length; index++)
			{
				if (index == 0)
				{
					ashiCandles.Add(new Candle()
					{
						Time = source[index].Time,
						Volume = source[index].Volume,
						Trades = source[index].Trades,
						Open = open?[index] ?? 0m,
						Close = close?[index] ?? 0m,
						High = high?[index] ?? 0m,
						Low = low?[index] ?? 0m
					});
				}
				else
				{
					ashiCandles.Add(new Candle()
					{
						Time = source[index].Time,
						Volume = source[index].Volume,
						Trades = source[index].Trades,
						Open = (open?[index - 1] ?? 0m + open?[index - 1] ?? 0) / 2,
						Close = (close?[index - 1] ?? 0 + close?[index - 1] ?? 0 + close?[index - 1] ?? 0 + close?[index - 1] ?? 0) / 4,
						High = new[] {high?[index - 1] ?? 0, high?[index - 1] ?? 0, high?[index - 1] ?? 0}.Max(),
						Low = new[] {low?[index - 1] ?? 0, low?[index - 1] ?? 0, low?[index - 1] ?? 0}.Min()
					});
				}
			}

			return ashiCandles;
		}
	}
}