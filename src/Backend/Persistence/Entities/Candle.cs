using System;
using Contracts;

namespace Persistence.Entities
{
	public class Candle : BaseEntity, ICandle
	{
		public DateTime Time { get; set; }
		public decimal High { get; set; }
		public decimal Low { get; set; }
		public decimal Open { get; set; }
		public decimal Close { get; set; }
		public decimal Volume { get; set; }
		public decimal Trades { get; set; }
	}
}