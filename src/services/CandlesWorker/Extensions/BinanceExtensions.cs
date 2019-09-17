using Binance.Net.Objects;
using core.Abstractions.TypeCodes;
using core.Infrastructure.Database.Entities;
using core.TypeCodes;
using System;

namespace CandlesWorker.Extensions
{
	public static class BinanceExtensions
	{
		public static Candle ToCandle (this BinanceStreamKline kline)
		{
			Candle candle = new Candle();
			candle.ExchangeCode = ExchangeCode.BINANCE.Code;
			candle.Symbol = kline.Symbol.ToUpper();
			candle.OpenTime = kline.OpenTime;
			candle.Open = kline.Open;
			candle.CloseTime = kline.CloseTime;
			candle.Close = kline.Close;
			candle.High = kline.High;
			candle.Low = kline.Low;
			candle.Volume = kline.Volume;
			candle.NumberOfTrades = kline.TradeCount;
			candle.PeriodCode = kline.Interval.ToPeriodCode().Code;
			candle.Time = new DateTime(kline.OpenTime.Year, kline.OpenTime.Month, kline.OpenTime.Day, kline.OpenTime.Hour, kline.OpenTime.Minute, 0);

			return candle;
		}

		public static Candle ToCandle (this BinanceKline kline, string symbol, IPeriodCode period)
		{
			Candle candle = new Candle();
			candle.ExchangeCode = ExchangeCode.BINANCE.Code;
			candle.Symbol = symbol;
			candle.OpenTime = kline.OpenTime;
			candle.Open = kline.Open;
			candle.CloseTime = kline.CloseTime;
			candle.Close = kline.Close;
			candle.High = kline.High;
			candle.Low = kline.Low;
			candle.Volume = kline.Volume;
			candle.NumberOfTrades = kline.TradeCount;
			candle.PeriodCode = period.Code;
			candle.Time = new DateTime(kline.OpenTime.Year, kline.OpenTime.Month, kline.OpenTime.Day, kline.OpenTime.Hour, kline.OpenTime.Minute, 0);

			return candle;
		}

		public static IPeriodCode ToPeriodCode (this KlineInterval interval)
		{
			switch (interval)
			{
				case KlineInterval.OneMinute:
					return PeriodCode.MINUTE;

				case KlineInterval.FiveMinutes:
					return PeriodCode.FIVE_MINUTES;

				case KlineInterval.FifteenMinutes:
					return PeriodCode.QUARTER_HOUR;

				case KlineInterval.ThirtyMinutes:
					return PeriodCode.HALF_HOUR;

				case KlineInterval.OneHour:
					return PeriodCode.HOUR;

				case KlineInterval.FourHour:
					return PeriodCode.FOUR_HOUR;

				case KlineInterval.OneDay:
					return PeriodCode.DAY;

				default:
					throw new Exception($"Not supported perion {interval.ToString()}");
			}
		}

		public static KlineInterval ToPeriodCode (this IPeriodCode period)
		{
			if (period == (IPeriodCode)PeriodCode.MINUTE)
			{
				return KlineInterval.OneMinute;
			}
			else if (period == (IPeriodCode)PeriodCode.FIVE_MINUTES)
			{
				return KlineInterval.FiveMinutes;
			}
			else if (period == (IPeriodCode)PeriodCode.QUARTER_HOUR)
			{
				return KlineInterval.FifteenMinutes;
			}
			else if (period == (IPeriodCode)PeriodCode.HALF_HOUR)
			{
				return KlineInterval.ThirtyMinutes;
			}
			else if (period == (IPeriodCode)PeriodCode.HOUR)
			{
				return KlineInterval.OneHour;
			}
			else if (period == (IPeriodCode)PeriodCode.FOUR_HOUR)
			{
				return KlineInterval.FourHour;
			}
			else if (period == (IPeriodCode)PeriodCode.DAY)
			{
				return KlineInterval.OneDay;
			}

			throw new Exception($"Not supported perion {period.ToString()}");
		}
	}
}
