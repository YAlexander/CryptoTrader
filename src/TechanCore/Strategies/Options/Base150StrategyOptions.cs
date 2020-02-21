using Contracts.Enums;
using Contracts.Trading;
using TechanCore.Enums;

namespace TechanCore.Strategies.Options
{
	public class Base150StrategyOptions : IStrategyOption
	{
		public int VeryFastSmaPeriod { get; set; }
		public int FastSmaPeriod { get; set; }
		public int SlowSmaPeriod { get; set; }
		public int VerySlowSmaPeriod { get; set; }
		
		public CandleVariables PriceToUse { get; set; } 
	}
}