using core.Abstractions;
using System;

namespace core.Infrastructure.Database.Entities
{
	public class Candle : BaseEntity, ICandle
	{
		public string Symbol { get; set; }

		public int ExchangeCode { get; set; }

		public DateTime Time { get; set; }

		public int PeriodCode { get; set; }

		public DateTime OpenTime { get; set; }

		public DateTime CloseTime { get; set; }

		public decimal Open { get; set; }

		public decimal Close { get; set; }

		public decimal High { get; set; }

		public decimal Low { get; set; }

		public decimal Volume { get; set; }

		public long NumberOfTrades { get; set; }
	}
}
