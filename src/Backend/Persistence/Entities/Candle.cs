using System;
using Contracts;
using Contracts.Enums;

namespace Persistence.Entities
{
	[Serializable]
	public class Candle : BaseEntity, ICandle
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
	}
}