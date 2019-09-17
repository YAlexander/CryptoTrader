using System;

namespace core.Trading.Models
{
	public class TradingForecast
	{
		public DateTime Created { get; } = DateTime.UtcNow;
		public int ExchangeCode { get; set; }
		public string Symbol { get; set; }
		public int ForecastCode { get; set; }
	}
}
