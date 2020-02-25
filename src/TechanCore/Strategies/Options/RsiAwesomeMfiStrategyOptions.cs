using Contracts.Trading;

namespace TechanCore.Strategies.Options
{
	public class RsiAwesomeMfiStrategyOptions : IStrategyOption
	{
		public int RsiPeriod { get; set; }
		
		public int MfiPeriod { get; set; }
		
		public int AwesomeFastPeriod { get; set; }
		public int AwesomeSlowPeriod { get; set; }

	}
}