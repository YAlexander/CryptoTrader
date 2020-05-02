using System;
using Abstractions.Enums;
using Common;
using TechanCore;
using TechanCore.Enums;

namespace Persistence.Entities
{
	[Serializable]
	public class Candle : BaseEntity<long>, ICandle
	{
		public Exchanges Exchange { get; set; }
		public Assets Asset1 { get; set; }
		public Assets Asset2 { get; set; }

		public DateTime Time { get; set; }
		public Timeframes TimeFrame { get; set; }
		public decimal High { get; set; }
		public decimal Low { get; set; }
		public decimal Open { get; set; }
		public decimal Close { get; set; }
		public decimal Volume { get; set; }
		public decimal Trades { get; set; }

		public Candle() { }

		public Candle(ICandle candle, GrainKeyExtension extension)
		{
			Exchange = extension.Exchange;
			Asset1 = extension.Asset1;
			Asset2 = extension.Asset2;

			Time = candle.Time;
			TimeFrame = candle.TimeFrame;
			High = candle.High;
			Low = candle.Low;
			Open = candle.Open;
			Close = candle.Close;
			Volume = candle.Volume;
			Trades = candle.Trades;
		}
	}
}