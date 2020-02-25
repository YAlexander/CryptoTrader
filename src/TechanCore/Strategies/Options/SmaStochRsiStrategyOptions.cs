using Contracts.Trading;

namespace TechanCore.Strategies.Options
{
	public class SmaStochRsiStrategyOptions : IStrategyOption
	{
		public int StochPeriod { get; set; }
		
		public int StochEmaPeriod { get; set; }
		
		public int SmaPeriod { get; set; }
		
		public int RsiPeriod { get; set; }
	}
}