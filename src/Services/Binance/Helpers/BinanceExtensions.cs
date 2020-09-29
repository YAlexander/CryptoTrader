using System;
using Abstractions.Enums;
using Binance.Net.Enums;
using Binance.Net.Interfaces;
using Binance.Net.Objects.Spot.MarketStream;
using Persistence.Entities;
using TechanCore.Enums;

namespace Binance.Helpers
{
	public static class BinanceExtensions
	{
		public static KlineInterval Map (this Timeframes timeFrame)
		{
			switch (timeFrame)
			{
				case Timeframes.MINUTE:
					return KlineInterval.OneMinute;

				case Timeframes.FIVE_MINUTES:
					return KlineInterval.FiveMinutes;

				case Timeframes.QUARTER_HOUR:
					return KlineInterval.FifteenMinutes;

				case Timeframes.HALF_HOUR:
					return KlineInterval.ThirtyMinutes;

				case Timeframes.HOUR:
					return KlineInterval.OneHour;

				case Timeframes.FOUR_HOUR:
					return KlineInterval.FourHour;

				case Timeframes.DAY:
					return KlineInterval.OneDay;

				default:
					throw new Exception("Unsupported interval");
			}
		}

		public static Timeframes Map (this KlineInterval timeFrame)
		{
			switch (timeFrame)
			{
				case KlineInterval.OneMinute:
					return Timeframes.MINUTE;

				case KlineInterval.FiveMinutes:
					return Timeframes.FIVE_MINUTES;

				case KlineInterval.FifteenMinutes:
					return Timeframes.QUARTER_HOUR;

				case KlineInterval.ThirtyMinutes:
					return Timeframes.HALF_HOUR;

				case KlineInterval.OneHour:
					return Timeframes.HOUR;

				case KlineInterval.FourHour:
					return Timeframes.FOUR_HOUR;

				case KlineInterval.OneDay:
					return Timeframes.DAY;

				default:
					throw new Exception("Unsupported period");
			}
		}

		public static TradeEntity Map (this BinanceStreamTrade binanceTrade, ExchangeSettings pair)
		{
			// TODO: Validate trade symbol
			TradeEntity trade = new TradeEntity();
			trade.Exchange = pair.Exchange;
			trade.Asset1 = pair.Asset1;
			trade.Asset2 = pair.Asset2;
			trade.Time = binanceTrade.TradeTime;
			trade.Price = binanceTrade.Price;
			trade.Quantity = binanceTrade.Quantity;
			
			return trade;
		}
		
		public static CandleEntity Map (this IBinanceStreamKline binanceCandle, ExchangeSettings pair)
		{
			CandleEntity candle = new CandleEntity();
			candle.Exchange = pair.Exchange;
			candle.Asset1 = pair.Asset1;
			candle.Asset2 = pair.Asset2;

			candle.Time = binanceCandle.CloseTime;
			candle.TimeFrame = pair.Timeframe;
			candle.High = binanceCandle.High;
			candle.Low = binanceCandle.Low;
			candle.Open = binanceCandle.Open;
			candle.Close = binanceCandle.Close;
			candle.Volume = binanceCandle.QuoteVolume;
			candle.Trades = binanceCandle.TradeCount;

			return candle;
		}

		public static string ToPair(this ExchangeSettings settings)
		{
			return $"{settings.Asset1}{settings.Asset2}";
		}
		
		public static FillPolitics Map (this TimeInForce item)
		{
			switch (item)
			{
				case TimeInForce.FillOrKill:
					return FillPolitics.FOK;
				case TimeInForce.ImmediateOrCancel:
					return FillPolitics.IOC;
				case TimeInForce.GoodTillCancel:
					return FillPolitics.GTC;
				default:
					throw new Exception($"Unsupported value {item}");
			}
		}

		public static TimeInForce Map (this FillPolitics item)
		{
			switch (item)
			{
				case FillPolitics.FOK:
					return TimeInForce.FillOrKill;
				case FillPolitics.IOC:
					return TimeInForce.ImmediateOrCancel;
				case FillPolitics.GTC:
					return TimeInForce.GoodTillCancel;
				default:
					throw new Exception($"Unsupported value {item}");
			}
		}
	}
}
