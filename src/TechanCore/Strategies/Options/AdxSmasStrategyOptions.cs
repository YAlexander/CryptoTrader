using Contracts.Trading;

namespace TechanCore.Strategies.Options
{
	public class AdxSmasStrategyOptions : IStrategyOption
	{
		public int FastSmaPeriod { get; set; }
		public int SlowSmaPeriod { get; set; }
		
		public int AdxPeriod { get; set; }
	}
}