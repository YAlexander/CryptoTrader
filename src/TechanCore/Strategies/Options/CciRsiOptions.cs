using Contracts.Trading;

namespace TechanCore.Strategies.Options
{
	public class CciRsiOptions : IStrategyOption
	{
		public int RsiPeriod { get; set; }
		public int CciPeriod { get; set; }
	}
}