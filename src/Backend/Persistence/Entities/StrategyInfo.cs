using Abstractions.Entities;
using Abstractions.Enums;

namespace Persistence.Entities
{
	public class StrategyInfo : IStrategyInfo
	{
		public Exchanges Exchange { get; set; }
		public Assets Asset1Code { get; set; }
		public Assets Asset2Code { get; set; }
		public string StrategyName { get; set; }
		public string StrategyClass { get; set; }
		public string Options { get; set; }
		public string DefaultOptions { get; set; }
		public string Class { get; set; }
		public int TimeFrame { get; set; }
		public bool TradeOnFlat { get; set; }
		public bool UseTrailingStop { get; set; }
		public bool UseMarginalTrading { get; set; }
		public int Leverage { get; set; }
		public bool IsDisabled { get; set; }
		public string[] Constraints { get; set; }
	}
}