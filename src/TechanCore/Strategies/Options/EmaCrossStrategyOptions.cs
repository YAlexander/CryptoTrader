using Contracts.Trading;

namespace TechanCore.Strategies.Options
{
	public class EmaCrossStrategyOptions : IStrategyOption
	{
		public int FastEmaPeriod { get; set; }
		public int SlowEmaPeriod { get; set; }
	}
}