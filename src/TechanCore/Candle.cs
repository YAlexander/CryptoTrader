using System;
using TechanCore.Enums;

namespace TechanCore
{
	public class Candle : ICandle
	{
		public DateTime Time { get; set; }
		public Timeframes TimeFrame { get; set; }
		public decimal High { get; set; }
		public decimal Low { get; set; }
		public decimal Open { get; set; }
		public decimal Close { get; set; }
		public decimal Volume { get; set; }
		public decimal Trades { get; set; }
	}
}