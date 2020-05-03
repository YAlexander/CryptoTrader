using Abstractions;
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
		
		public int TimeFrame { get; set; }
		
		public bool IsDisabled { get; set; }
		
		public string RiskManagerName { get; set; }
		public string RiskManagerOptions { get; set; }
		public bool IsShortAllowed { get; set; }
	}
}