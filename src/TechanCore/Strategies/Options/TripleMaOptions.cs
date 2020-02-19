using Contracts.Trading;

namespace TechanCore.Strategies.Options
{
	public class TripleMaOptions : IStrategyOption
	{
		public int FastSmaPeriod { get; set; }
		public int SlowSmaPeriod { get; set; }
		public int EmaPeriod { get; set; }
	}
}