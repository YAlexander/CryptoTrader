namespace Persistence.Entities
{
	public class StrategyInfo
	{
		public int ExchangeId { get; set; }
		
		public int Asset1Code { get; set; }
		
		public int Asset2Code { get; set; }
		
		public int StrategyId { get; set; }
		
		public string StrategyName { get; set; }
		
		public string Class { get; set; }
		
		public int TimeFrame { get; set; }
		
		public bool UseHeikenAshiCandles { get; set; }
		public bool SmoothHeikenAshiCandles { get; set; }
		
		public bool IsDisabled { get; set; }
	}
}