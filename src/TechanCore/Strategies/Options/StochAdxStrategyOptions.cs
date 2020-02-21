using Contracts.Trading;

namespace TechanCore.Strategies.Options
{
	public class StochAdxStrategyOptions : IStrategyOption
	{
		public int StochPeriod { get; set; }
		public int StochEmaPeriod { get; set; }

		public int AdxPeriod { get; set; }
	}
}